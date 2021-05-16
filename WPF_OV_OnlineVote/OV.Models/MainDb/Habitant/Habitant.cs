using System;
using System.Collections.Generic;
using System.Text;

namespace OV.Models.MainDb.Habitant
{
    public class Habitant
    {
        public int Id { get; set; }
        public int tblUser_UID { get; set; }
        public User.User User { get; set; }
    }
}
