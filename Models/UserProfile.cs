using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        [Required]
        public int UserId { get; set; }
        
        public User User { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }
    }
}
