namespace ADManager.Database.Models
{
    public class LockedOutUser : AppDbSetBase
    {
        public string Sid { get; set; }
        public DateTime Added { get; set; }
    }
}
