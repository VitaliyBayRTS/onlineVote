using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.AutonomousCommunity
{
    public interface IAutonomousCommunityService
    {
       Task<IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity>> FindAsync(AutonomousCommunityFilter filter,
                                                                                            CancellationToken cancellationToken);

    }
    public class AutonomousCommunityService : IAutonomousCommunityService
    {
        private IOvMainDbContext dbContext;
        private IFindAutonomousCommunityService _findAutonomousCommunityService;
        public AutonomousCommunityService()
        {
            dbContext = new CreateDbContext().getOvMainDbContext();
            _findAutonomousCommunityService = new FindAutonomousCommunityService(new FindAutonomousCommunityDataService(dbContext));
        }

        public async Task<IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity>> FindAsync(
                AutonomousCommunityFilter filter, CancellationToken cancellationToken)
        {
            return await _findAutonomousCommunityService.FindAsync(filter, cancellationToken);
        }
    }
}
