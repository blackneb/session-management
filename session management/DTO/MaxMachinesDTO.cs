using System.ComponentModel.DataAnnotations;

namespace session_management.DTO
{
    public class MaxMachinesDTO
    {
        [Required]
        public int MaxMachines { get; set; }
    }
}
