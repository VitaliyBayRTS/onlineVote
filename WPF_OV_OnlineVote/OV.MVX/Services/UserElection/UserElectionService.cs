using OV.MainDb.Configuration;
using OV.MainDb.UserElection.Find;
using OV.MVX.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.UserElection
{
    public interface IUserElectionService
    {
        Task<IEnumerable<OV.Models.MainDb.UserElection.UserElection>> FindAsync(int tblUser_UID, int tblElection_UID, CancellationToken cancellationToken);
    }
    public class UserElectionService : IUserElectionService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindUserElectionService _findUserElectionService;

        public UserElectionService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();

            _findUserElectionService = new FindUserElectionService(new FindUserElectionDataService(_ovMainDbContextFactory));
        }

        public async Task<IEnumerable<OV.Models.MainDb.UserElection.UserElection>> FindAsync(int tblUser_UID, int tblElection_UID, CancellationToken cancellationToken)
        {
            return await _findUserElectionService.FindAsync(tblUser_UID, tblElection_UID, cancellationToken);
        }
    }
}
