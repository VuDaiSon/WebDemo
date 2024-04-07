using main_prj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace main_prj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {                       
            ViewBag.userName = HttpContext.Session.GetString("UserName");
            ViewBag.isAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");  
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public IActionResult AdminDashboard()
        {
            ViewBag.userName = HttpContext.Session.GetInt32("UserName");
            
            return View();
        }
    }
}
