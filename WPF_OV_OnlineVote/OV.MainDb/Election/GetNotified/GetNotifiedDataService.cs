using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.GetNotified
{
    public interface IGetNotifiedDataService
    {
        Task<IEnumerable<PersistedElection>> GetNotifiedAsync(CancellationToken cancellationToken);
    }
    public class GetNotifiedDataService : IGetNotifiedDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public GetNotifiedDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedElection>> GetNotifiedAsync(CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var elections = ovMainDbContext.Elections
                                .AsNoTracking()
                                .Include(e => e.Type)
                                .Where(e => e.IsNotified != null && e.IsNotified.Value);

            var electionToReturn = await elections.ToListAsync(cancellationToken);

            electionToReturn.ForEach(e => e.Type.Election = null);

            return electionToReturn;
        }
    }
}
