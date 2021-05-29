using System.Collections.Generic;

namespace OV.Models.MainDb.AutonomousCommunity
{
    public class AutonomousCommunity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Province.Province>? Provinces { get; set; } = default!;
        public IEnumerable<Election.Election>? Election { get; set; } = default!;
    }
}
