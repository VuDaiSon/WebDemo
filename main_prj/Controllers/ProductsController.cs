using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using main_prj.Models;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace main_prj.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {        
        private readonly ComputerShopContext _context;

        public ProductsController(ComputerShopContext context)
        {
            _context = context;
        }

        [Route("products")]
        public IActionResult ViewProducts()
        {
            var query = _context.Products.Select(p => p);
            return View(query);
        }

        [Route("ProductsByCategory/{categoryId::int}")]
        public IActionResult ProductsByCategory (int categoryId)
        {
            var productList = _context.Products.Where(n => n.CategoryId == categoryId).ToList();
                                    
            return View(productList);
        }

        [Route("search")]
        public IActionResult SearchProducts (string search)
        {
            var query = _context.Products.Where(n => n.ProductName.Contains(search));
            return View(query);
        }

        [Route("ProductDetails/{productId::int}")]
        public IActionResult ProductsDetails(int productId)
        {
            var product = _context.Products.FirstOrDefault(n => n.ProductId == productId);
            var monitor = _context.Monitors.FirstOrDefault(n => n.ProductId == productId);
            var mouse = _context.Mice.FirstOrDefault(m => m.ProductId == productId);
            var headphone = _context.Headphones.FirstOrDefault(n => n.ProductId == productId);
            var keyboard = _context.Keyboards.FirstOrDefault(n => n.ProductId == productId);
            
            var path = "wwwroot/ProductImages/" + productId.ToString();
            var images = Directory.GetFiles(path).Select(Path.GetFileName).ToList();
            var viewModel = new ViewModel
            {
                Product = product,
                Monitor = monitor,
                Mouse = mouse,
                Headphone = headphone,
                Keyboard = keyboard,
                Images = images
            };
            return View(viewModel);
        }

        public class PriceRange()
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        [Route("GetFilteredProducts")]
        [HttpPost]
        public IActionResult GetFilteredProducts([FromBody] FilteredData filteredData)
        {
            var filteredProducts = _context.Products.ToList();
            if (filteredData.PriceRange != null && filteredData.PriceRange.Count > 0
                && !filteredData.PriceRange.Contains("all"))
            {
                List<PriceRange> priceRanges = new List<PriceRange>();
                foreach (var priceRange in filteredData.PriceRange)
                {
                    var value = priceRange.Split('-').ToArray();
                    PriceRange range = new PriceRange();
                    range.Min = Convert.ToInt16(value[0]);
                    range.Max = Convert.ToInt16(value[1]); 
                    priceRanges.Add(range);

                }
                filteredProducts = filteredProducts.Where(n => priceRanges
                    .Any(r => n.Price >= r.Min * 1000000 && n.Price <= r.Max * 1000000)).ToList();
            }
            return PartialView("PartialView/_ProductPartial",filteredProducts);
        }

        // GET: Products
        
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }
            else return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        [Route("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductModificationModel model = new ProductModificationModel();
            model.Product = product;
            switch(product.CategoryId)
            {
                case 1:
                    model.Monitor = _context.Monitors.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Màn hình";
                    break;
                case 2:
                    model.Headphone = _context.Headphones.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Tai nghe";
                    break;
                case 3:
                    model.Mouse = _context.Mice.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Chuột";
                    break;
                case 4:
                    model.Keyboard = _context.Keyboards.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Bàn phím";
                    break;
                default:
                    break;
            }

            var previewImagePath = Path.Combine("wwwroot", "ProductImages", "PreviewImages", id.ToString());
            var detailImagesPath = Path.Combine("wwwroot", "ProductImages", id.ToString());
            if (!System.IO.Directory.Exists(previewImagePath))
            {
                return NotFound();
            }
            
            if (!System.IO.Directory.Exists(detailImagesPath))
            {
                return NotFound();
            }
            ViewData["detailImages"] = Directory.GetFiles(detailImagesPath).Select(Path.GetFileName).ToList();
            return View(model);
        }


        // GET: Products/Create
        
        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }
            ViewData["categoryList"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]*/
        /*public async Task<IActionResult> Create([Bind("ProductId,ProductName,Price,StockQuantity,ProductDescription,Warranty,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }*/


        /*[HttpPost]
        public IActionResult Create (Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Products") });
            } 
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }*/

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(ProductModificationModel model)
        {
            if (ModelState.IsValid)
            {
                //Info handler
                if (_context.Products.FirstOrDefault(p => p.ProductName == model.Product.ProductName) != null)
                {
                    return BadRequest("Ambiguous product name");
                }

                _context.Products.Add(model.Product);
                _context.SaveChanges();
                var id = _context.Products.FirstOrDefault(p => p.ProductName == model.Product.ProductName).ProductId;

                switch (model)
                {
                    case { Mouse: not null }:
                        model.Mouse.ProductId = id;
                        _context.Mice.Add(model.Mouse);
                        break;
                    case { Headphone: not null }:
                        model.Headphone.ProductId = id;
                        _context.Headphones.Add(model.Headphone);
                        break;
                    case { Keyboard: not null }:
                        model.Keyboard.ProductId = id;
                        _context.Keyboards.Add(model.Keyboard);
                        break;
                    case { Monitor: not null }:
                        model.Monitor.ProductId = id;                        
                        _context.Monitors.Add(model.Monitor);
                        break;
                    default:
                        // Handle the case when none of the properties are not null
                        break;
                }
                _context.SaveChanges();

                //File handler
                try
                {
                    var previewImageName = "Preview.webp";
                    var previewImagePath = Path.Combine("wwwroot", "ProductImages", "PreviewImages", id.ToString(), previewImageName);
                                        
                    Directory.CreateDirectory(Path.GetDirectoryName(previewImagePath));

                    using (var image = Image.Load(model.PreviewImage.OpenReadStream()))
                    {                        
                        var width = 600;
                        var height = 600;
                        image.Mutate(x => x.Resize(width, height));

                        // Save the image directly to the file
                        image.Save(previewImagePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                    }

                    foreach(var item in model.DetailImages)
                    {
                        var imageName = item.FileName;
                        var imagePath = Path.Combine("wwwroot", "ProductImages", id.ToString(), imageName);

                        Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                        using (var image = Image.Load(item.OpenReadStream()))
                        {
                            var width = 600;
                            var height = 600;
                            image.Mutate(x => x.Resize(width, height));

                            image.Save(imagePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return BadRequest($"Error: {ex.Message}");
                }

                /*return Json(new { success = true, redirectUrl = Url.Action("Index", "Products") });*/
                return RedirectToAction("Index", "Products");   
            }
            else
            {
                // If model state is not valid, return validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                return View(errors);
            }            
        }

        [HttpGet]
        [Route("GetSpecsCreateView")]
        public IActionResult GetSpecsCreateView(int categoryId)
        {
            if (categoryId == 1)
            {
                return PartialView("PartialView/_CreateMonitor");
            }
            else if (categoryId == 2)
            {
                return PartialView("PartialView/_CreateHeadphone");
            }
            else if (categoryId == 3)
            {
                return PartialView("PartialView/_CreateMouse");
            }
            else if (categoryId == 4)
            {
                return PartialView("PartialView/_CreateKeyboard");
            }
            else return Content("");
        }

        // GET: Products/Edit/5
        [HttpGet]
        [Route("Edit/{id::int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ProductModificationModel model = new ProductModificationModel();
            model.Product = product;

            /*var productByCategory = _context.Products.Where(p => p.CategoryId == model.Product.CategoryId).ToList();
            ViewData["productIds"] = new SelectList(productByCategory, "ProductId", "ProductName");*/

            switch (product.CategoryId)
            {
                case 1:
                    model.Monitor = _context.Monitors.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Màn hình";
                    break;
                case 2:
                    model.Headphone = _context.Headphones.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Tai nghe";
                    break;
                case 3:
                    model.Mouse = _context.Mice.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Chuột";
                    break;
                case 4:
                    model.Keyboard = _context.Keyboards.FirstOrDefault(m => m.ProductId == id);
                    ViewBag.CategoryName = "Bàn phím";
                    break;
                default:
                    break;
            }

            ViewData["categoryList"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            var previewImagePath = Path.Combine("wwwroot", "ProductImages", "PreviewImages", id.ToString());
            var detailImagesPath = Path.Combine("wwwroot", "ProductImages", id.ToString());
            if (!System.IO.Directory.Exists(previewImagePath))
            {
                return NotFound();
            }

            if (!System.IO.Directory.Exists(detailImagesPath))
            {
                return NotFound();
            }
            ViewData["detailImages"] = Directory.GetFiles(detailImagesPath).Select(Path.GetFileName).ToList();
            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        [Route("Edit/{id::int}")]
        public async Task<IActionResult> Edit(int id, ProductModificationModel model)
        {
            Product product = model.Product;
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var previewImagePath = Path.Combine("wwwroot", "ProductImages", "PreviewImages", id.ToString());
                var detailImagesPath = Path.Combine("wwwroot", "ProductImages", id.ToString());
                
                if (model.PreviewImage != null)
                {
                    try
                    {
                        //Delete exsiting files
                        foreach (var image in Directory.GetFiles(previewImagePath))
                        {
                            System.IO.File.Delete(image);
                        }
                        foreach (var image in Directory.GetFiles(detailImagesPath))
                        {
                            System.IO.File.Delete(image);
                        }

                        //Add replacements
                        var filePath = Path.Combine(previewImagePath, "Preview.webp");
                        using (var image = Image.Load(model.PreviewImage.OpenReadStream()))
                        {
                            var width = 600;
                            var height = 600;
                            image.Mutate(x => x.Resize(width, height));

                            // Save the image directly to the file
                            image.Save(filePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while replacing images: {ex.Message}");
                    }
                }

                if (model.DetailImages != null)
                {
                    try
                    {
                        foreach (var image in Directory.GetFiles(detailImagesPath))
                        {
                            System.IO.File.Delete(image);
                        }

                        foreach (var item in model.DetailImages)
                        {
                            var imageName = item.FileName;
                            var imagePath = Path.Combine("wwwroot", "ProductImages", id.ToString(), imageName);

                            using (var image = Image.Load(item.OpenReadStream()))
                            {
                                var width = 600;
                                var height = 600;
                                image.Mutate(x => x.Resize(width, height));

                                image.Save(imagePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while replacing images: {ex.Message}");
                    }
                }

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]       
        [Route("MonitorEdit/{id::int}")]
        public async Task<IActionResult> MonitorEdit(int id, [Bind("ProductId,Brand,Model,Size,AspectRatio,Resolution,RefreshRate,MonitorId")] main_prj.Models.Monitor monitor)
        {
            if (id != monitor.MonitorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Monitors.FirstOrDefault(m => m.MonitorId == id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", monitor.ProductId);
            return View(monitor);
        }

        [HttpPost]
        [Route("HeadphoneEdit/{id::int}")]
        public async Task<IActionResult> HeadphoneEdit(int id, [Bind("ProductId,Brand,Model,Compatibilities,ConnectionType,Battery,Accessories,Microphone,HeadphoneType,Color,HeadphoneId")] Headphone headphone)
        {
            if (id != headphone.HeadphoneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headphone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Headphones.FirstOrDefault(h => h.HeadphoneId == id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", headphone.ProductId);
            return View(headphone);
        }

        [HttpPost]
        [Route("KeyboardEdit/{id::int}")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Brand,Model,Color,ConnectionType,Battery,Weight,KeyboardType,Switch,Led,KeyboardId")] Keyboard keyboard)
        {
            if (id != keyboard.KeyboardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keyboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Keyboards.FirstOrDefault(k => k.KeyboardId == id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", keyboard.ProductId);
            return View(keyboard);
        }

        [HttpPost]
        [Route("MouseEdit/{id::int}")]
        public async Task<IActionResult> MouseEdit(int id, [Bind("ProductId,Brand,Model,Weight,Resolution,ConnectionType,Battery,Color,MouseId")] Mouse mouse)
        {
            if (id != mouse.MouseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Mice.FirstOrDefault(m => m.MouseId == id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", mouse.ProductId);
            return View(mouse);
        }

        // GET: Products/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [Route("DeleteConfirmed")]
        
        public IActionResult DeleteConfirmed([FromBody]JObject data)
        {
            var id = int.Parse(data["id"].ToString());
            var product = _context.Products.FirstOrDefault(c => c.ProductId == id);
                        
            switch(product.CategoryId)
            {
                case 1:                    
                    var monitor = _context.Monitors.FirstOrDefault(m => m.ProductId == product.ProductId);
                    _context.Monitors.Remove(monitor);
                    
                    break;
                case 2:
                    var headphone = _context.Headphones.FirstOrDefault(m => m.ProductId == product.ProductId);
                    _context.Headphones.Remove(headphone);
                    
                    break;
                case 3:
                    var mouse = _context.Mice.FirstOrDefault(m => m.ProductId == product.ProductId);
                    _context.Mice.Remove(mouse);
                    
                    break;
                case 4:
                    var keyboard = _context.Keyboards.FirstOrDefault(m => m.ProductId == product.ProductId);
                    _context.Keyboards.Remove(keyboard);
                    
                    break;
                default:
                    break;
            }
            
            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                var pImagePath = Path.Combine("wwwroot", "ProductImages", "PreviewImages", id.ToString());
                var dImagePath = Path.Combine("wwwroot", "ProductImages", id.ToString());
                if (Directory.Exists(pImagePath) && Directory.Exists(dImagePath))
                {
                    Directory.Delete(pImagePath, true);
                    Directory.Delete(dImagePath, true);
                }
                else
                {
                    return NotFound("Image folders not found");
                }
                return Json(new { success = true, returnUrl = Url.Action("Index", "Products") });
            } catch (DbUpdateException ex)
            {
                return Json(new { success = false, errors = ex.Message });
            }                                              
        }

        //Utility Methods
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        
    }
}
