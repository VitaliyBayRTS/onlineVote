using System;

namespace OV.Services.ReferenceNumber
{
    public class GenerateReferenceNumber
    {
        public static string Get()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Substring(0, 9);
            return GuidString;
        }
    }
}
