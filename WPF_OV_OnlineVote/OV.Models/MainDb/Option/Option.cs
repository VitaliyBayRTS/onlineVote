namespace OV.Models.MainDb.Option
{
    public class Option
    {
        public int? Id { get; set; }
        public int tblElection_UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Votes { get; set; }

        public Election.Election Election { get; set; }
    }
}
