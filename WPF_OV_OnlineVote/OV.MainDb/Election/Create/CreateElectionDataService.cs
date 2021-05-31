using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.MainDb.Election.Models.Public;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Create
{
    public interface ICreateElectionDataService
    {
        Task<PersistedElection> CreateAsync(CandidateElection candidate, CancellationToken cancellationToken);
    }
    public class CreateElectionDataService : ICreateElectionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public CreateElectionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<PersistedElection> CreateAsync(CandidateElection candidate, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            PersistedElection election = new PersistedElection()
            {
                Name = candidate.Name,
                Description = candidate.Description,
                tblAutonomousCommunity_UID = candidate.tblAutonomousCommunity_UID,
                tblProvince_UID = candidate.tblProvince_UID,
                InitDate = candidate.InitDate,
                FinalizeDate = candidate.FinalizeDate,
                tblType_UID = candidate.tblType_UID
            };

            var newElection = ovMainDbContext.Elections.Add(election);

            await ovMainDbContext.SaveChangesAsync(cancellationToken);

            return newElection.Entity;
        }
    }
}
