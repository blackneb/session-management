namespace session_management.DTO
{
    public class KeyModelDTO
    {
        public int KeyID { get; set; }
        public string KeyValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MaxMachines { get; set; }
        public int UsedMachines { get; set; }
    }
}
