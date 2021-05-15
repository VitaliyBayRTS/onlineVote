using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

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
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["OvMainDb"].ConnectionString);
            return new OvMainDbContext(optionsBuilder.Options);
        }
    }
}
