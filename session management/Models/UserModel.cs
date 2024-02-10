using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsEmailVerified { get; set; }
    }
}
