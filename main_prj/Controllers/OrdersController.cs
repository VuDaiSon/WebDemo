using main_prj.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace main_prj.Controllers
{
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly ComputerShopContext _context;
        public OrdersController(ComputerShopContext context)
        {
            _context = context;
        }
       
        public int CalculateShipingFee (int totalValue)
        {
            int shippingFee = 0;
            if (totalValue < 3000000)
            {
                shippingFee = 15000;
            }
            else if (totalValue < 5000000)
            {
                shippingFee = 20000;
            }
            else if (totalValue < 7500000)
            {
                shippingFee = 30000;
            }
            else shippingFee = 0;
            return shippingFee;
        }

        [HttpGet]
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                TempData["ErrorMessage"] = "Cần đăng nhập để thực hiện chức năng này";
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.Status == "Active");

            List<CartDetail> cartDetails = _context.CartDetails.Where(c => c.CartId == cart.CartId).ToList();

            if (cart == null || cart.CartDetails.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống. Hãy thêm sản phẩm vào giỏ hàng để tiến hành thanh toán";
                return RedirectToAction("Index", "Home");
            }
            
            List<Product> products = new List<Product>();
            
            foreach (CartDetail cartDetail in cartDetails)
            {
                var query = _context.Products.FirstOrDefault(p => p.ProductId == cartDetail.ProductId);
                products.Add(query);
            }

            int subtotal = 0;
            for (int i = 0; i < cartDetails.Count(); i++)
            {
                subtotal += cartDetails[i].Quantity * (int)products[i].Price;
            }
            
            int shippingFee = CalculateShipingFee(subtotal);

            CheckoutViewModel viewModel = new CheckoutViewModel { 
                User = user,
                Cart = cart,
                CartDetails = cartDetails,
                Products = products,
                Subtotal = subtotal,
                ShippingFee = shippingFee
            };
            return View(viewModel);
        }

        [Route("ConfirmOrder")]
        [HttpPost]
        public IActionResult ConfirmOrder([FromBody] JObject data)
        {
            Order order = new Order();            
            order.UserId = int.Parse(data["userId"].ToString());
            order.CartId = int.Parse(data["cartId"].ToString());
            order.OrderDate = DateTimeOffset.Parse(data["orderDate"].ToString(), null, DateTimeStyles.RoundtripKind);
            order.TotalValue = int.Parse(data["totalValue"].ToString());
            order.ShippingAddress = data["address"].ToString();
            order.ContactNumber = data["contactNumber"].ToString();
            order.ShippingFee = int.Parse(data["shippingFee"].ToString());
            var payment = data["paymentMethod"].ToString();
            if (payment == "paypal")
            {
                order.PaymentMethod = "Paypal";
            }
            else if (payment == "directcheck")
            {
                order.PaymentMethod = "Thanh toán khi nhận hàng";
            }
            else order.PaymentMethod = "Chuyển khoản";
            order.Receiver = data["receiver"].ToString();
            order.Status = "Chờ xác nhận";

            _context.Orders.Add(order);
            _context.SaveChanges();
            
            _context.Carts.FirstOrDefault(c => c.CartId == order.CartId).Status = "Checked out";
            _context.SaveChanges();
            return Ok();          
        }

        [Route("ClientIndex")]
        public IActionResult ClientIndex ()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Cần đăng nhập để thực hiện chức năng này";
                return RedirectToAction("Index", "Home");
            }                         
           
            var query = _context.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderDate);
            return View(query.ToList());
        }

        [Route("ClientDetails/{id::int}")]
        public IActionResult ClientDetails (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.CartId == order.CartId);
            List<CartDetail> cartDetails = _context.CartDetails.Where(c => c.CartId == cart.CartId).ToList();


            List<Product> products = new List<Product>();

            foreach (CartDetail cartDetail in cartDetails)
            {
                var query = _context.Products.FirstOrDefault(p => p.ProductId == cartDetail.ProductId);
                products.Add(query);
            }

            int subtotal = 0;
            for (int i = 0; i < cartDetails.Count(); i++)
            {
                subtotal += cartDetails[i].Quantity * (int)products[i].Price;
            }

            int shippingFee = CalculateShipingFee(subtotal);

            CheckoutViewModel viewModel = new CheckoutViewModel
            {
                User = user,
                Cart = cart,
                Order = order,
                CartDetails = cartDetails,
                Products = products,
                Subtotal = subtotal,
                ShippingFee = shippingFee
            };
            return View(viewModel);           
        }

        [Route("CancelOrder/{id::int}")]
        public IActionResult CancelOrder (int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            try
            {
                order.Status = "Đã hủy";
                _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction("ClientIndex", "Orders");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Route("AdminIndex")]
        public IActionResult AdminIndex()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }
            var query = _context.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(query);
        }

        [Route("AdminEdit/{id::int}")]
        public IActionResult AdminEdit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }

            if (_context.Orders.Find(id) == null)
            {
                return NotFound();
            }

            List<string> strings = new List<string>
            {
                "Chờ xác nhận",
                "Đã xác nhận",
                "Đang đóng gói",
                "Đã bàn giao cho đơn vị vân chuyển",
                "Đang giao hàng",
                "Đã nhận hàng"
            };
            SelectList orderStatuses = new SelectList  (strings);
            ViewData["orderStatuses"] = orderStatuses;
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            return View(order);
        }

        [HttpPost]
        [Route("AdminEdit/{id::int}")]
        public IActionResult AdminEdit(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction("AdminIndex", "Orders");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
