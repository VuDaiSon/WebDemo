using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using main_prj.Models;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace main_prj.Controllers
{
    [Route("Categories")]    
    
    public class CategoriesController : Controller
    {
        private readonly ComputerShopContext _context;

        public CategoriesController(ComputerShopContext context)
        {
            _context = context;
        }

        // GET: Categories
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        [Route("Details/{id::int}")]
        public async Task<IActionResult> Details(int? id)
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

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Route("Create")]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (_context.Users.FirstOrDefault(u => u.UserId == userId && u.IsAdmin == true) == null)
            {
                return View("AccessDeniedView");
            }
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CategoryModificationModel model)
        {
            Category category = model.Category;
            if (ModelState.IsValid)
            {              
                _context.Add(category);
                await _context.SaveChangesAsync();

                var id = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryName == category.CategoryName);
                try
                {
                    if (model.imageFile == null)
                    {
                        return BadRequest();
                    }
                    var imageName = category.CategoryName.ToLower() + ".webp";
                    category.CategoryImage = imageName;
                    var imagePath = Path.Combine("wwwroot", "ProductImages", "CategoryImages", category.CategoryId.ToString(), imageName);
                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                    using (var image = Image.Load(model.imageFile.OpenReadStream()))
                    {
                        var width = 1200;
                        var height = 800;
                        image.Mutate(x => x.Resize(width, height));

                        // Save the image directly to the file
                        image.Save(imagePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return BadRequest($"Error: {ex.Message}");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
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

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryModificationModel categoryModification = new CategoryModificationModel();
            categoryModification.Category = category;
            return View(categoryModification);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id::int}")]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Edit(int id, CategoryModificationModel model)
        {
            Category category = model.Category;
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.imageFile != null)
                {                    
                    try
                    {
                        var imageName = category.CategoryName.ToLower() + ".webp";
                        category.CategoryImage = imageName;
                        var imagePath = Path.Combine("wwwroot", "ProductImages", "CategoryImages", category.CategoryId.ToString(), imageName);

                        System.IO.File.Delete(imagePath);

                        using (var image = Image.Load(model.imageFile.OpenReadStream()))
                        {
                            var width = 1200;
                            var height = 800;
                            image.Mutate(x => x.Resize(width, height));

                            // Save the image directly to the file
                            image.Save(imagePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder());
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while replacing images: {ex.Message}");
                    }
                }

                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        [Route("Delete/{id::int}")]
        public async Task<IActionResult> Delete(int? id)
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

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Delete/{id::int}")]
       /* [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var imagePath = Path.Combine("wwwroot", "ProductImages", "CategoryImages", category.CategoryId.ToString(), category.CategoryImage);
            if (Directory.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
