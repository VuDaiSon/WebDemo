using main_prj.Models;
using Microsoft.AspNetCore.Mvc;

namespace main_prj.Components
{
    public class RecentOrders : ViewComponent
    {
        private readonly ComputerShopContext _context;
        public RecentOrders(ComputerShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Orders.OrderByDescending(o => o.OrderDate).Take(5).ToList());
        }
    }
}
