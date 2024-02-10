namespace session_management.DTO
{
    public class KeyExtensionModelDTO
    {
        public int ExtensionID { get; set; }
        public int KeyID { get; set; }
        public DateTime NewExpiryDate { get; set; }
        public DateTime ExtensionDate { get; set; }
    }
}
