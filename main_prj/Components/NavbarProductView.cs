using main_prj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_prj.Components
{
    public class NavbarProductView : ViewComponent
    {
        private readonly ComputerShopContext _context;

        public NavbarProductView(ComputerShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Categories.ToList());
        }
    }
}
