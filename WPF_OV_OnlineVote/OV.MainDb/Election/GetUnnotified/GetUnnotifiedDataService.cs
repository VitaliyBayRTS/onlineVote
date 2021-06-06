using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.GetUnnotified
{
    public interface IGetUnnotifiedDataService
    {
        Task<IEnumerable<PersistedElection>> GetUnnotifiedAsync(CancellationToken cancellationToken);
    }
    public class GetUnnotifiedDataService : IGetUnnotifiedDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public GetUnnotifiedDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedElection>> GetUnnotifiedAsync(CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var elections = ovMainDbContext.Elections
                                .AsNoTracking()
                                .Include(e => e.Type)
                                .Where(e => (e.IsNotified == null && e.FinalizeDate < DateTime.Today)
                    || (e.IsNotified.Value && e.FinalizeDate < DateTime.Today));

            var electionToReturn = await elections.ToListAsync(cancellationToken);

            electionToReturn.ForEach(e => e.Type.Election = null);

            return electionToReturn;
        }
    }
}
