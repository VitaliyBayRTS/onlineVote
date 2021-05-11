using OV.MainDb.Province.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Province.Find
{
    public interface IFindProvinceService
    {
        Task<IEnumerable<OV.Models.MainDb.Province.Province>> FindAsync(ProvinceFilter filter, CancellationToken cancellationToken);
    }

    public class FindProvinceService : IFindProvinceService
    {
        private readonly IFindProvinceDataService _findProvinceDataService;

        public FindProvinceService(IFindProvinceDataService findProvinceDataService)
        {
            _findProvinceDataService = findProvinceDataService ??
                                                  throw new ArgumentNullException(nameof(findProvinceDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Province.Province>> FindAsync(ProvinceFilter filter, CancellationToken cancellationToken)
        {
            var autonomousCommunity = await _findProvinceDataService.FindAsync(filter, cancellationToken);
            return autonomousCommunity.Select(ac => ac.ToProvince()).ToList();
        }
    }
}
