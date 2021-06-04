using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.UserElection.Find
{
    public interface IFindUserElectionService
    {
        Task<IEnumerable<OV.Models.MainDb.UserElection.UserElection>> FindAsync(int tblUser_UID, int tblElection_UID, CancellationToken cancellationToken);
    }
    public class FindUserElectionService : IFindUserElectionService
    {
        private readonly IFindUserElectionDataService _findUserElectionDataService;

        public FindUserElectionService(IFindUserElectionDataService findUserElectionDataService)
        {
            _findUserElectionDataService = findUserElectionDataService ??
                                                  throw new ArgumentNullException(nameof(findUserElectionDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.UserElection.UserElection>> FindAsync(int tblUser_UID, int tblElection_UID, CancellationToken cancellationToken)
        {
            var userElections = await _findUserElectionDataService.FindAsync(tblUser_UID, tblElection_UID, cancellationToken);
            return userElections.Select(ue => ue.ToUserElection());
        }
    }
}
