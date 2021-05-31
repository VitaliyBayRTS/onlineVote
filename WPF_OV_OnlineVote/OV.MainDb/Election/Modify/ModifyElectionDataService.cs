using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.MainDb.Election.Modify.Models.Public;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Modify
{
    public interface IModifyElectionDataService
    {
        Task<PersistedElection> ModifyAsync(ModifyElectionCandidate candidate, CancellationToken cancellationToken);
    }

    public class ModifyElectionDataService : IModifyElectionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public ModifyElectionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<PersistedElection> ModifyAsync(ModifyElectionCandidate candidate, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var election = ovMainDbContext.Elections.First(e => e.Id == candidate.Id);

            election.Name = candidate.Name;
            election.Description = candidate.Description;
            election.InitDate = candidate.InitDateTime;
            election.FinalizeDate = candidate.FinalizeDateTime;

            await ovMainDbContext.SaveChangesAsync(cancellationToken);

            return ovMainDbContext.Elections.First(e => e.Id == candidate.Id);
        }
    }
}
