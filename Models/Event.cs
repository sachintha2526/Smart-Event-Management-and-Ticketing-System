using System.ComponentModel.DataAnnotations;

namespace SmartEventSystem.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }
    }
}
