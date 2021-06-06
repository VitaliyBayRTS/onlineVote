using OV.MainDb.Configuration;

namespace OV.DbRemoteConfigurationService.DbService
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
