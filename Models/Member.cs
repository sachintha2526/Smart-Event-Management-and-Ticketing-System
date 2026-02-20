using System.ComponentModel.DataAnnotations;

namespace SmartEventSystem.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Preferences { get; set; } // Comma-separated or serialized JSON

        public string Role { get; set; } = "Member"; // Default Role
    }
}
