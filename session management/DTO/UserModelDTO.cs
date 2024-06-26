﻿namespace session_management.DTO
{
    public class UserModelDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
