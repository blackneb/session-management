namespace session_management.Models
{
    public class KeyExtensionModel
    {
        public int ExtensionID { get; set; }
        public int KeyID { get; set; }
        public DateTime NewExpiryDate { get; set; }
        public DateTime ExtensionDate { get; set; }
    }
}
