using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.GetUnnotified
{
    public interface IGetUnnotifiedService
    {
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetUnnotifiedAsync(CancellationToken cancellationToken);
    }
    public class GetUnnotifiedService : IGetUnnotifiedService
    {
        private readonly IGetUnnotifiedDataService _getUnnotifiedDataService;

        public GetUnnotifiedService(IGetUnnotifiedDataService getUnnotifiedDataService)
        {
            _getUnnotifiedDataService = getUnnotifiedDataService ??
                                                  throw new ArgumentNullException(nameof(getUnnotifiedDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetUnnotifiedAsync(CancellationToken cancellationToken)
        {
            var persistedElection = await _getUnnotifiedDataService.GetUnnotifiedAsync(cancellationToken);
            return persistedElection.Select(e => e.ToElection());
        }
    }
}
