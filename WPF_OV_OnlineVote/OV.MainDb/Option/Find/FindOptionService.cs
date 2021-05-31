using OV.MainDb.Option.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Find
{
    public interface IFindOptionService
    {
        Task<IEnumerable<OV.Models.MainDb.Option.Option>> FindAsync(OptionFilter filter, CancellationToken cancellationToken);
    }

    public class FindOptionService : IFindOptionService
    {
        private readonly IFindOptionDataService _findOptionDataService;

        public FindOptionService(IFindOptionDataService findOptionDataService)
        {
            _findOptionDataService = findOptionDataService ??
                                                  throw new ArgumentNullException(nameof(findOptionDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Option.Option>> FindAsync(OptionFilter filter, CancellationToken cancellationToken)
        {
            var options = await _findOptionDataService.FindAsync(filter, cancellationToken);
            return options.Select(o => o.ToOption());
        }
    }
}
