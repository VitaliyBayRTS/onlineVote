using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.AutonomousCommunity.Find
{
    public interface IFindAutonomousCommunityService
    {
        Task<IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity>> FindAsync(AutonomousCommunityFilter filter, 
                                                                                         CancellationToken cancellationToken);
    }

    public class FindAutonomousCommunityService : IFindAutonomousCommunityService
    {
        private readonly IFindAutonomousCommunityDataService _findAutonomousCommunityDataService;

        public FindAutonomousCommunityService(IFindAutonomousCommunityDataService findAutonomousCommunityDataService)
        {
            _findAutonomousCommunityDataService = findAutonomousCommunityDataService ?? 
                                                  throw new ArgumentNullException(nameof(findAutonomousCommunityDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity>> FindAsync(AutonomousCommunityFilter filter, CancellationToken cancellationToken)
        {
            var autonomousCommunity = await _findAutonomousCommunityDataService.FindAsync(filter, cancellationToken);
            return autonomousCommunity.Select(ac => ac.ToAutonomousCommunity()).ToList();
        }
    }
}
