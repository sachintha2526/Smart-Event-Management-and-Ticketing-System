using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEventSystem.Data;
using SmartEventSystem.Models;

namespace SmartEventSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index(string searchString, string category, decimal? minPrice, decimal? maxPrice)
        {
            var events = from e in _context.Events
                         select e;

            if (!string.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.Name.Contains(searchString) || s.Venue.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                events = events.Where(x => x.Category == category);
            }

            if (minPrice.HasValue)
            {
                events = events.Where(x => x.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                events = events.Where(x => x.Price <= maxPrice);
            }

            // Populate categories for the view
            ViewBag.Categories = await _context.Events.Select(e => e.Category).Distinct().ToListAsync();

            return View(await events.ToListAsync());
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }
    }
}
