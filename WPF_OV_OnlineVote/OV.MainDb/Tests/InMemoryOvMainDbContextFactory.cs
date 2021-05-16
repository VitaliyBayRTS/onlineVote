using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Tests
{
    internal class InMemoryOvMainDbContextFactory : IOvMainDbContextFactory
    {
        private readonly string _dbName;

        public InMemoryOvMainDbContextFactory(string dbName)
        {
            _dbName = dbName;
        }

        public IOvMainDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OvMainDbContext>();
            optionsBuilder.UseInMemoryDatabase(_dbName);
            return new OvMainDbContext(optionsBuilder.Options);
        }
    }
}
