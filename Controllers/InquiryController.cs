using Microsoft.AspNetCore.Mvc;
using SmartEventSystem.Data;
using SmartEventSystem.Models;

namespace SmartEventSystem.Controllers
{
    public class InquiryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inquiry/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquiry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Message")] Inquiry inquiry)
        {
            inquiry.InquiryDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(inquiry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Confirmation));
            }
            return View(inquiry);
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
