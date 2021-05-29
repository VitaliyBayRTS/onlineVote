using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Election.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Find
{
    public interface IFindElectionDataService
    {
        Task<IEnumerable<PersistedElection>> FindAsync(ElectionFilter filter, CancellationToken cancellationToken);
    }
    public class FindElectionDataService : IFindElectionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindElectionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedElection>> FindAsync(ElectionFilter filter, CancellationToken cancellationToken)
        {
            var _ovMainDbContext = _ovMainDbContextFactory.Create();

            var elections = _ovMainDbContext.Elections
                                .AsNoTracking()
                                .Include(e => e.Type)
                                .Include(e => e.Province)
                                .Include(e => e.AutonomousCommunity)
                                .Include(e => e.Organizers)
                                .Where(e => e.Type != null);

            var electionToReturn = await elections.ToListAsync(cancellationToken);

            if (!filter.TypeIncluded)
            {
                electionToReturn.ForEach(e => e.Type = null);
            }
            else
            {
                electionToReturn.ForEach(e => { if (e.Type != null) e.Type.Election = null; });
            }

            if (!filter.ACIncluded)
            {
                electionToReturn.ForEach(e => e.AutonomousCommunity = null);
            }
            else
            {
                electionToReturn.ForEach(e => { if (e.AutonomousCommunity != null) e.AutonomousCommunity.Elections = null; });
            }

            if (!filter.ProvinceIncluded)
            {
                electionToReturn.ForEach(e => e.Province = null);
            } else
            {
                electionToReturn.ForEach(e => { if (e.Province != null) e.Province.Elections = null; });
            }

            if (!filter.OrganizersIncluded)
            {
                electionToReturn.ForEach(e => e.Organizers = null);
            }
            else
            {
                electionToReturn.ForEach(e => e.Organizers.ToList().ForEach(o => o.Election = null));
            }

            return electionToReturn;
        }
    }
}
