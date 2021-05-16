using System.Configuration;

namespace OV.MainDb.Configuration
{
    public class ConnectionStringConfig
    {
        public string Xds { get; set; } = default!;
        public string OvMainDb { get; set; } = ConfigurationManager.ConnectionStrings["OvMainDb"].ConnectionString;
        public string Authentication { get; set; } = default!;
        public string Upload { get; set; } = default!;
        public string Security { get; set; } = default!;
    }
}
