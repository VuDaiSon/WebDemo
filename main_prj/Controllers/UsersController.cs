using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using main_prj.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace main_prj.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly ComputerShopContext _context;

        public UsersController(ComputerShopContext context)
        {
            _context = context;
        }

        //Login
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User user)
        {
            /*string email = data["email"].ToString();
            string password = data["password"].ToString();*/
            if (!String.IsNullOrEmpty(user.Email) && !String.IsNullOrEmpty(user.Password))
            {
                var query = _context.Users.FirstOrDefault(n => n.Email == user.Email
                    && n.Password == user.Password);
                if (query == null)
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu sai");
                    return View();
                }
                else
                {
                    string? userName = query.UserName;
                    int userId = query.UserId;
                    HttpContext.Session.SetInt32("UserId", userId);
                    HttpContext.Session.SetString("UserName", userName);      
                    if (query.IsAdmin == true)
                    {
                        HttpContext.Session.SetString("IsAdmin", "true");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else return View();
        }

        [Route("Logout")]        
        public IActionResult Logout ()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".GearClub.Session");
            return RedirectToAction("Index", "Home");
        }

        //Validation
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsUnique(string email)
        {
            bool isUnique = !_context.Users.Any(n => n.Email.ToString() == email);
            return Json(isUnique);
        }

        //Register
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(User user)
        {
            if (String.IsNullOrEmpty(user.UserName) || String.IsNullOrEmpty(user.Password) 
                || String.IsNullOrEmpty(user.Email))
            {
                //ViewData["error"] = "Vui lòng điền vào trường còn thiếu";
                return View("Login");
            }
            else
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return View("Login");
                }
                catch (Exception)
                {
                    //ViewData["error"] = ex.Message;
                    return View("Login");
                }
            }
        }

        // GET: Users
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        [Route("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Email,PhoneNumber,Address,Password,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Route("ClientEdit/{id::int}")]
        public async Task<IActionResult> ClientEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("ClientEdit/{id::int}")]
        public async Task<IActionResult> ClientEdit(int id, [Bind("UserId, UserName,Email,PhoneNumber,Address,Password")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Route("Edit/{id::int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id::int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Email,PhoneNumber,Address,Password,IsAdmin")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        [Route("Delete/{id::int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Delete/{id::int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
