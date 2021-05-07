using Microsoft.EntityFrameworkCore;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.AutonomousCommunity.Find
{
    public interface IFindAutonomousCommunityDataService
    {
        Task<IEnumerable<PersistedAutonomousCommunity>> FindAsync(AutonomousCommunityFilter filter, CancellationToken cancellationToken);
    }
    public class FindAutonomousCommunityDataService : IFindAutonomousCommunityDataService
    {
        private IOvMainDbContext _ovMainDbContext;
        public FindAutonomousCommunityDataService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }

        public async Task<IEnumerable<PersistedAutonomousCommunity>> FindAsync(AutonomousCommunityFilter filter, CancellationToken cancellationToken)
        {
            var autonomousCommunities = _ovMainDbContext.AutonomousCommunities
                            .Include(ac => ac.Provinces)
                            .Where(ac => true);
            if(filter.Id != default(int))
            {
                autonomousCommunities = autonomousCommunities.Where(ac => ac.Id == filter.Id);
            }

            if(!string.IsNullOrEmpty(filter.Name))
            {
                autonomousCommunities = autonomousCommunities.Where(ac => ac.Name.Equals(filter.Name));
            }

            var autonomousCommunitiesToReturn = await autonomousCommunities.OrderBy(ac => ac.Name).ToListAsync(cancellationToken);

            if(!filter.ProvinceIncluded)
            {
                autonomousCommunitiesToReturn.ForEach(ac => ac.Provinces = null);
            }

            return autonomousCommunitiesToReturn;
        }
    }
}
