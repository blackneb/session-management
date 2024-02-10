using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class AdminModel
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [StringLength(50)]
        public string AdminUsername { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string AdminEmail { get; set; }

        [StringLength(20)]
        public string AdminPhoneNumber { get; set; }

        [StringLength(255)]
        public string AdminAddress { get; set; }
    }
}
