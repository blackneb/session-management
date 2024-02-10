﻿using System.ComponentModel.DataAnnotations;

namespace session_management.Models
{
    public class AdminModel
    {
        [Key]
        public int AdminID { get; set; }
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPhoneNumber { get; set; }
        public string AdminAddress { get; set; }
    }
}
