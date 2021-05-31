using System;

namespace OV.MainDb.Election.Modify.Models.Public
{
    public class ModifyElectionCandidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime InitDateTime { get; set; }
        public DateTime FinalizeDateTime { get; set; }
    }
}
