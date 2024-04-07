using Microsoft.AspNetCore.Mvc;
using main_prj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace main_prj.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ComputerShopContext _context;
        public CartController(ComputerShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetInt32("UserId");
            /* var query = _context.Carts.Where(c => c.UserId == user).SelectMany(c => c.CartDetails).ToList();*/
            var query = _context.Carts
                        .Include(c => c.CartDetails)
                        .ThenInclude(cd => cd.Product) // Eager loading Product for each CartDetail
                        .Where(c => c.UserId == user && c.Status.ToString() == "Active") // Filter by userId and status
                        .SelectMany(c => c.CartDetails) // Flatten cart details
                        .ToList();
            return View(query);
        }

        [HttpPost]
        [Route("AddToCart")]
        public IActionResult AddToCart([FromBody] JObject data)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Cần đăng nhập để thực hiện chức năng này";
                return RedirectToAction("Index", "Home");
            }
            var userCart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.Status.ToString() == "Active");
            var quantity = int.Parse(data["quantity"].ToString());
            var productId = int.Parse(data["productId"].ToString());

            if (userCart == null)
            {
                Cart cart = new Cart
                {
                    UserId = (int)userId,
                    Status = "Active"
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var query = _context.Carts.FirstOrDefault(c => c.Status.ToString() == "Active" && c.UserId == userId);
            var cartDetails = _context.CartDetails.Where(c => c.CartId == query.CartId).ToList();
            var lineToUpdate = cartDetails.FirstOrDefault(c => c.ProductId == productId);
            if (lineToUpdate != null)
            {
                lineToUpdate.Quantity += quantity;
                _context.SaveChanges();
            }
            else
            {
                CartDetail line = new CartDetail
                {
                    CartId = query.CartId,
                    Quantity = quantity,
                    ProductId = productId
                };
                _context.CartDetails.Add(line);
                _context.SaveChanges();
            }

            return Ok("Quantity: " + quantity + " and ProductId: " + productId);
        }

        [HttpPost]
        [Route("UpdateCart")]
        public IActionResult UpdateCart([FromBody] JObject data)
        {
            var cartDetailid = int.Parse(data["id"].ToString());
            var quantity = int.Parse(data["quantity"].ToString());
            var cartLine = _context.CartDetails.FirstOrDefault(c => c.CartDetailId == cartDetailid);
            if (cartLine == null)
            {
                return NotFound();
            }
            else
            {
                cartLine.Quantity += quantity;
                if (cartLine.Quantity == 0) 
                {
                    cartLine.Quantity = 1;
                }                
                _context.Update(cartLine);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpPost]
        [Route("RemoveCartLine")]
        public IActionResult RemoveCartLine([FromBody] JObject data)
        {
            int cartDetailId = int.Parse(data["id"].ToString());
            var query = _context.CartDetails.FirstOrDefault(c => c.CartDetailId == cartDetailId);
            if (query == null)
            {
                return NotFound();
            }
            else
            {
                _context.CartDetails.Remove(query);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
}
