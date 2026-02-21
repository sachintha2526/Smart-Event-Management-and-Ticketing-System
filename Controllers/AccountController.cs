using Microsoft.AspNetCore.Mvc;
using SmartEventSystem.Data;
using SmartEventSystem.Models;
using SmartEventSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SmartEventSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Member member)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_context.Members.Any(m => m.Email == member.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(member);
                }

                _context.Add(member);
                _context.SaveChanges();
                
                // Set session
                HttpContext.Session.SetInt32("MemberId", member.MemberId);
                HttpContext.Session.SetString("FullName", member.FullName);
                HttpContext.Session.SetString("Role", member.Role);

                return RedirectToAction("Index", "Home");
            }
            return View(member);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Members.FirstOrDefault(m => m.Email == model.Email && m.Password == model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("MemberId", user.MemberId);
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("Role", user.Role);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
