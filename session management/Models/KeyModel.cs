using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class KeyModel
    {
        [Key]
        public int KeyID { get; set; }
        public string KeyValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MaxMachines { get; set; }
    }
}
