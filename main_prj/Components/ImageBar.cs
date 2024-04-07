using main_prj.Models;
using Microsoft.AspNetCore.Mvc;

namespace main_prj.Components
{
    public class ImageBar:ViewComponent
    {
        private readonly ComputerShopContext _context;
        public ImageBar(ComputerShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var query = _context.Categories.ToList();
            List<int> productCount = new List<int>();
            foreach (var category in query)
            {
                int count = _context.Products.Count(p => p.CategoryId == category.CategoryId);
                productCount.Add(count);
            }
            ViewBag.ProductCount = productCount;    
            return View(query);
        }
    }
}
