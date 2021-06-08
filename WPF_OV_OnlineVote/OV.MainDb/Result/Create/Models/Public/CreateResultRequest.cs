namespace OV.MainDb.Result.Create.Models.Public
{
    public class CreateResultRequest
    {
        public int TotalVotes { get; set; }
        public int Winner_UID { get; set; }
        public int TotalHabitants { get; set; }
        public int TotalHabitantsThatParticipate { get; set; }
        public int TblElection_UID { get; set; }
    }
}
