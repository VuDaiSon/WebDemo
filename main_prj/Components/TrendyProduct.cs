using main_prj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_prj.Components
{
    public class TrendyProduct : ViewComponent
    {
        private readonly ComputerShopContext _context;

        public TrendyProduct(ComputerShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var query = _context.Products.OrderBy(p => Guid.NewGuid()).Take(8).ToList();
            return View(query);
        }
    }
}
