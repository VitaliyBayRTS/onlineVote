namespace OV.Models.MainDb.Result
{
    public class Result
    {
        public int? Id { get; set; }
        public int TotalVotes { get; set; }
        public int? Winner_UID { get; set; }
        public int TotalHabitants { get; set; }
        public int HabitantsThatParticipate { get; set; }
        public Election.Election Election { get; set; }
    }
}
