namespace OV.Models.MainDb.SuperAdmin
{
    public class SuperAdmin
    {
        public int Id { get; set; }
        public int tblUser_UID { get; set; }
        public string ReferenceNumber { get; set; }
        public User.User User { get; set; }
    }
}
