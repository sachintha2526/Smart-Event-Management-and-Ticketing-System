using SmartEventSystem.Models;

namespace SmartEventSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any events.
            if (context.Events.Any())
            {
                return;   // DB has been seeded
            }

            var events = new Event[]
            {
                new Event{Name="City Jazz Festival", Category="Music", Description="A night of smooth jazz and blues.", Venue="Central Park Amphitheater", EventDate=DateTime.Now.AddDays(7), Price=50.00m, TotalSeats=200, AvailableSeats=200},
                new Event{Name="Modern Art Exhibition", Category="Art", Description="Showcasing contemporary artists from around the globe.", Venue="Metropolitan Art Gallery", EventDate=DateTime.Now.AddDays(14), Price=25.00m, TotalSeats=100, AvailableSeats=100},
                new Event{Name="Shakespeare in the Park", Category="Theatre", Description="A classic performance of Hamlet.", Venue="City Gardens", EventDate=DateTime.Now.AddDays(21), Price=30.00m, TotalSeats=150, AvailableSeats=150},
                new Event{Name="Tech Innovation Summit", Category="Conference", Description="Leading tech minds share the future.", Venue="Convention Center", EventDate=DateTime.Now.AddDays(30), Price=150.00m, TotalSeats=500, AvailableSeats=500},
                new Event{Name="Food & Wine Carnival", Category="Food", Description="Taste the best cuisines and wines.", Venue="Downtown Square", EventDate=DateTime.Now.AddDays(10), Price=15.00m, TotalSeats=1000, AvailableSeats=1000}
            };

            foreach (Event e in events)
            {
                context.Events.Add(e);
            }
            context.SaveChanges();
        }
    }
}
