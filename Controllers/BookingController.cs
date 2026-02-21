using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEventSystem.Data;
using SmartEventSystem.Models;

namespace SmartEventSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking/Create?eventId=5
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

            var booking = new Booking
            {
                EventId = @event.EventId,
                Event = @event
            };
            
            ViewBag.EventName = @event.Name;
            ViewBag.Price = @event.Price;

            return View(booking);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,SeatType,Quantity")] Booking booking)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            booking.MemberId = memberId.Value;
            booking.BookingDate = DateTime.Now;

            var @event = await _context.Events.FindAsync(booking.EventId);
            if (@event == null)
            {
                return NotFound();
            }

            if (@event.AvailableSeats < booking.Quantity)
            {
                ModelState.AddModelError("Quantity", "Not enough seats available.");
                booking.Event = @event;
                ViewBag.EventName = @event.Name;
                ViewBag.Price = @event.Price;
                return View(booking);
            }

            // Calculate Total Amount
            // Simple logic: VIP = +50%, Regular = Base, Economy = -20%
            decimal multiplier = 1.0m;
            switch(booking.SeatType)
            {
                case "VIP": multiplier = 1.5m; break;
                case "Economy": multiplier = 0.8m; break;
                default: multiplier = 1.0m; break;
            }



            booking.TotalAmount = @event.Price * booking.Quantity * multiplier;

            ModelState.Remove("MemberId");
            ModelState.Remove("TotalAmount");
            ModelState.Remove("Event");
            ModelState.Remove("Member");
            ModelState.Remove("BookingDate");

            if (ModelState.IsValid)
            {
                @event.AvailableSeats -= booking.Quantity;
                _context.Update(@event);
                
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Confirmation), new { id = booking.BookingId });
            }
            
            booking.Event = @event;
            ViewBag.EventName = @event.Name;
            ViewBag.Price = @event.Price;
            return View(booking);
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.BookingId == id);
                
            if (booking == null)
            {
                return NotFound();
            }
            
            // Security check: only show if it belongs to current user
            if (booking.MemberId != HttpContext.Session.GetInt32("MemberId"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(booking);
        }
    }
}
