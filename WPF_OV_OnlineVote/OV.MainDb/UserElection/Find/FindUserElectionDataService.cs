using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.UserElection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.UserElection.Find
{
    public interface IFindUserElectionDataService
    {
        Task<IEnumerable<PersistedUserElection>> FindAsync(int tblUser_UID, int tblElection_UID, CancellationToken cancellationToken);
    }
    public class FindUserElectionDataService : IFindUserElectionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindUserElectionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedUserElection>> FindAsync(int tblUser_UID, int tblElection_UID, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();
            var userElections = ovMainDbContext.UserElections
                        .Where(ue => ue.TblUser_UID == tblUser_UID && ue.TblElection_UID == tblElection_UID);

            return await userElections.ToListAsync(cancellationToken);
        }
    }
}
