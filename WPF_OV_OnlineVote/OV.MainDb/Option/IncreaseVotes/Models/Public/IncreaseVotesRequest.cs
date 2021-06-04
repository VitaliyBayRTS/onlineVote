namespace OV.MainDb.Option.IncreaseVotes.Models.Public
{
    public class IncreaseVotesRequest
    {
        public int TblUser_UID { get; set; }
        public int TblElection_UID { get; set; }
        public int TblOption_UID { get; set; }
    }
}
