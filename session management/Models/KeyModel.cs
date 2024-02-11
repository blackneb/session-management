using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class KeyModel
    {
        [Key]
        public int KeyID { get; set; }

        [Required]
        public string KeyValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public int MaxMachines { get; set; }
        public int UsedMachines { get; set; }
    }
}
