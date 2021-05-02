using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Configuration;
using OV.MVX.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OV.MVX.Services.AutonomousCommunity
{
    public interface IAutonomousCommunityService
    {
        IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity> Find();

    }
    public class AutonomousCommunityService : IAutonomousCommunityService
    {
        private IOvMainDbContext dbContext;
        private IFindAutonomousCommunityService _findAutonomousCommunityService;
        public AutonomousCommunityService()
        {
            dbContext = new CreateDbContext().getOvMainDbContext();
            _findAutonomousCommunityService = new FindAutonomousCommunityService(dbContext);
        }

        public IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity> Find()
        {
            return _findAutonomousCommunityService.Find();
        }
    }
}
