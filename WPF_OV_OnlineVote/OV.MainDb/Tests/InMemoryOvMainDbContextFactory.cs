using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            return new OvMainDbContext(optionsBuilder.Options);
        }
    }
}
