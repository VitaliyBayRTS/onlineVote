using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.GetNotified
{
    public interface IGetNotifiedService
    {
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetNotifiedAsync(CancellationToken cancellationToken);
    }
    public class GetNotifiedService : IGetNotifiedService
    {
        private readonly IGetNotifiedDataService _getNotifiedDataService;

        public GetNotifiedService(IGetNotifiedDataService getNotifiedDataService)
        {
            _getNotifiedDataService = getNotifiedDataService ??
                                                  throw new ArgumentNullException(nameof(getNotifiedDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> GetNotifiedAsync(CancellationToken cancellationToken)
        {
            var persistedElection = await _getNotifiedDataService.GetNotifiedAsync(cancellationToken);
            return persistedElection.Select(e => e.ToElection());
        }
    }
}
