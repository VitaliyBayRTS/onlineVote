using System.Collections.Generic;

namespace OV.Models.MainDb.Province
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AutonomousCommunity.AutonomousCommunity AutonomousCommunity { get; set; }
        public ICollection<User.User> Users{ get; set; }
        public ICollection<Election.Election> Elections{ get; set; }
    }
}
