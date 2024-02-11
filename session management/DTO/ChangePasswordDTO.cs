using System.ComponentModel.DataAnnotations;

namespace session_management.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
