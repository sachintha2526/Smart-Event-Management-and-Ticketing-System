using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEventSystem.Data;
using SmartEventSystem.Models;

namespace SmartEventSystem.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Review/Create?eventId=5
        public async Task<IActionResult> Create(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetInt32("MemberId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                EventId = @event.EventId,
                Event = @event
            };
            
            ViewBag.EventName = @event.Name;

            return View(review);
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Rating,Comment")] Review review)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            review.MemberId = memberId.Value;
            review.ReviewDate = DateTime.Now;

            review.MemberId = memberId.Value;
            review.ReviewDate = DateTime.Now;

            ModelState.Remove("Member");
            ModelState.Remove("Event");
            ModelState.Remove("MemberId");
            ModelState.Remove("ReviewDate");

            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Event", new { id = review.EventId });
            }
            
            // Reload event info for view
             var @event = await _context.Events.FindAsync(review.EventId);
             review.Event = @event;
             ViewBag.EventName = @event.Name;
             
            return View(review);
        }
    }
}
