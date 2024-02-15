namespace session_management.DTO
{
    public class KeyInfoDTO
    {
        public int MaxMachines { get; set; }
        public int UsedMachines { get; set; }
        public int LeftMachines { get; set; }
        public List<int> UserIDs { get; set; }
    }
}
