using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Configuration
{
    public interface IOvMainDbContextFactory
    {
        IOvMainDbContext Create();
    }
    public class OvMainDbContextFactory : IOvMainDbContextFactory
    {
        private readonly OvMainDatabase _ovMainDatabase;
        public OvMainDbContextFactory(OvMainDatabase ovMainDatabase)
        {
            _ovMainDatabase = ovMainDatabase ?? throw new ArgumentNullException(nameof(ovMainDatabase));
        }

        public IOvMainDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OvMainDbContext>();
            //optionsBuilder.UseSqlServer(_ovMainDatabase.GetConnectionString());
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-O2K81VH;Initial Catalog=OV_MainDb;Integrated Security=True");
            return new OvMainDbContext(optionsBuilder.Options);
        }
    }
}
