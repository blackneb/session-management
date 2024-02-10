using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class KeyExtensionModel
    {
        [Key]
        public int ExtensionID { get; set; }

        public int KeyID { get; set; }

        [Required]
        public DateTime NewExpiryDate { get; set; }

        [Required]
        public DateTime ExtensionDate { get; set; }

        public KeyModel Key { get; set; }
    }
}
