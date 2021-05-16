namespace OV.Models.MainDb.Organizer
{
    public class Organizer
    {
        public int Id { get; set; }
        public int tblUser_UID { get; set; }
        public int tblElection_UID { get; set; }
        public string ReferenceNumber { get; set; }
        public User.User User { get; set; }
    }
}
