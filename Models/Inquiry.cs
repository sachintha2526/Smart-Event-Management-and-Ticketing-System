using System.ComponentModel.DataAnnotations;

namespace SmartEventSystem.Models
{
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime InquiryDate { get; set; } = DateTime.Now;
    }
}
