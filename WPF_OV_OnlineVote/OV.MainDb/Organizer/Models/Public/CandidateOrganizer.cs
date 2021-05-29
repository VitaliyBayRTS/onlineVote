namespace OV.MainDb.Organizer.Models.Public
{
    public class CandidateOrganizer
    {
        public int? Id { get; set; }
        public int tblUser_UID { get; set; }
        public int tblElection_UID { get; set; }
        public string ReferenceNumber { get; set; }

        public CandidateOrganizer(OV.Models.MainDb.Organizer.Organizer organizer)
        {
            Id = organizer.Id;
            tblUser_UID = organizer.tblUser_UID;
            tblElection_UID = organizer.tblElection_UID;
            ReferenceNumber = organizer.ReferenceNumber;
        }

        public CandidateOrganizer()
        {
        }
    }
}
