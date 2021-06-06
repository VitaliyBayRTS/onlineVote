using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Result.GetResult.Models.Public
{
    public class GetResultResponse
    {
        public int TblElection_UID { get; set; }
        public int TotalHabitant { get; set; }
        public int HabitantCountThatParticipate { get; set; }
        public IEnumerable<OV.Models.MainDb.Option.Option> Options { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
    }
}
