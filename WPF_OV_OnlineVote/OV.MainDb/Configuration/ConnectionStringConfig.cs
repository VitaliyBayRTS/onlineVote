using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Configuration
{
    public class ConnectionStringConfig
    {
        public string Xds { get; set; } = default!;
        public string OvMainDb { get; set; } = "Data Source=DESKTOP-O2K81VH;Initial Catalog=OV_MainDb;Integrated Security=True";
        public string Authentication { get; set; } = default!;
        public string Upload { get; set; } = default!;
        public string Security { get; set; } = default!;
    }
}
