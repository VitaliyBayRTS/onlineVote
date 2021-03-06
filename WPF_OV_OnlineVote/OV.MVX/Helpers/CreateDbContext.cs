using OV.MainDb.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MVX.Helpers
{
    internal interface ICreateDbContext
    {
        IOvMainDbContext getOvMainDbContext();
        IOvMainDbContextFactory getOvMainDbContextFactory();
    }
    public class CreateDbContext : ICreateDbContext
    {
        private readonly IOvMainDbContext _ovMainDbContext;

        public CreateDbContext()
        {
            _ovMainDbContext = new OvMainDbContextFactory(new OvMainDatabase()).Create();
        }

        public IOvMainDbContext getOvMainDbContext()
        {
            return _ovMainDbContext;
        }

        public IOvMainDbContextFactory getOvMainDbContextFactory()
        {
            return new OvMainDbContextFactory(new OvMainDatabase());
        }
    }
}
