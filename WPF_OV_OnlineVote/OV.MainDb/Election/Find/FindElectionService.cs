using OV.MainDb.Election.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Find
{
    public interface IFindElectionService
    {
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindAsync(ElectionFilter filter, CancellationToken cancellationToken);
    }
    public class FindElectionService : IFindElectionService
    {
        private readonly IFindElectionDataService _findElectionsDataService;

        public FindElectionService(IFindElectionDataService findElectionsDataService)
        {
            _findElectionsDataService = findElectionsDataService ??
                                                  throw new ArgumentNullException(nameof(findElectionsDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindAsync(ElectionFilter filter, CancellationToken cancellationToken)
        {
            var elections = await _findElectionsDataService.FindAsync(filter, cancellationToken);
            return elections.Select(e => e.ToElection());
        }
    }
}
