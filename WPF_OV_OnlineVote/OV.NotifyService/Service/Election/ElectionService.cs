using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Find;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Election.GetNotified;
using OV.MainDb.Election.GetUnnotified;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.NotifyService.Service.Election
{
    public interface IElectionService
    {
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetNotifiedAsync(CancellationToken cancellationToken);
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetUnnotifiedAsync(CancellationToken cancellationToken);
    }
    public class ElectionService : IElectionService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IGetNotifiedService _getNotifiedService;
        private IGetUnnotifiedService _getUnnotifiedService;
        public ElectionService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            _getNotifiedService = new GetNotifiedService(new GetNotifiedDataService(_ovMainDbContextFactory));
            _getUnnotifiedService = new GetUnnotifiedService(new GetUnnotifiedDataService(_ovMainDbContextFactory));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetNotifiedAsync(CancellationToken cancellationToken)
        {
            return await _getNotifiedService.GetNotifiedAsync(cancellationToken);
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetUnnotifiedAsync(CancellationToken cancellationToken)
        {
            return await _getUnnotifiedService.GetUnnotifiedAsync(cancellationToken);
        }
    }
}
