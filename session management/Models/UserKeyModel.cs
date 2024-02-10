using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class UserKeyModel
    {
        [Key]
        public int UserKeyID { get; set; }

        public int UserID { get; set; }

        public int KeyID { get; set; }

        public int MachinesUsed { get; set; }

        public UserModel User { get; set; }

        public KeyModel Key { get; set; }
    }
}
