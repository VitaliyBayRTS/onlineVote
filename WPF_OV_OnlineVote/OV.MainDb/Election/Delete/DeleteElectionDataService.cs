using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Delete
{
    public interface IDeleteElectionDataService
    {
        Task<bool> DeleteAsync(int electionId, CancellationToken cancellationToken);
    }
    public class DeleteElectionDataService : IDeleteElectionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public DeleteElectionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<bool> DeleteAsync(int electionId, CancellationToken cancellationToken)
        {
            var _ovMainDbContext = _ovMainDbContextFactory.Create();

            var election = _ovMainDbContext.Elections.First(e => e.Id == electionId);

            _ovMainDbContext.Elections.Remove(election);

            await _ovMainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
