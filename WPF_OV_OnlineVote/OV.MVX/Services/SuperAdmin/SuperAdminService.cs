using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.SuperAdmin.Find;
using OV.MainDb.SuperAdmin.Find.Models.Public;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.SuperAdmin
{
    public interface ISuperAdminService
    {
        Task<IEnumerable<OV.Models.MainDb.SuperAdmin.SuperAdmin>> FindAsync(SuperAdminFilter filter, CancellationToken cancellationToken);
    }
    public class SuperAdminService : ISuperAdminService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindSuperAdminService _findSuperAdminService;
        public SuperAdminService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();

            var findSuperAdminDataService = new FindSuperAdminDataService(_ovMainDbContextFactory);
            _findSuperAdminService = new FindSuperAdminService(findSuperAdminDataService);
        }

        public Task<IEnumerable<OV.Models.MainDb.SuperAdmin.SuperAdmin>> FindAsync(SuperAdminFilter filter, CancellationToken cancellationToken)
        {
            return _findSuperAdminService.FindAsync(filter, cancellationToken);
        }
    }
}
