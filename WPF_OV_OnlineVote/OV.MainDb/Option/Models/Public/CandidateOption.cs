namespace OV.MainDb.Option.Models.Public
{
    public class CandidateOption
    {
        public int? Id { get; set; }
        public int tblElection_UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
