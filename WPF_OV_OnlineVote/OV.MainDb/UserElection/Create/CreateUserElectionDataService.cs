using OV.MainDb.Configuration;
using OV.MainDb.UserElection.Models;
using OV.MainDb.UserElection.Models.Public;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.UserElection.Create
{
    public interface ICreateUserElectionDataService
    {
        Task<PersistedUserElection> CreateAsync(CandidateUserElection candidate, CancellationToken cancellationToken);
    }
    public class CreateUserElectionDataService : ICreateUserElectionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IOvMainDbContext _ovMainDbContext;

        public CreateUserElectionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
            _ovMainDbContext = _ovMainDbContextFactory.Create();
        }

        public CreateUserElectionDataService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }

        public async Task<PersistedUserElection> CreateAsync(CandidateUserElection candidate, CancellationToken cancellationToken)
        {
            PersistedUserElection persistedUserElection = new PersistedUserElection()
            {
                Id = candidate.Id,
                TblUser_UID = candidate.TblUser_UID,
                TblElection_UID = candidate.TblElection_UID
            };

            var newUserElection = _ovMainDbContext.UserElections.Add(persistedUserElection);

            await _ovMainDbContext.SaveChangesAsync(cancellationToken);

            return newUserElection.Entity;

        }
    }
}
