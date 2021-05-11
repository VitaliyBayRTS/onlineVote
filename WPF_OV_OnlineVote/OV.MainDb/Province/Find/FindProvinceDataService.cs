using Microsoft.EntityFrameworkCore;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Find.Models.Public;
using OV.MainDb.Province.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Province.Find
{
    public interface IFindProvinceDataService
    {
        Task<IEnumerable<PersistedProvince>> FindAsync(ProvinceFilter filter, CancellationToken cancellationToken);
    }
    public class FindProvinceDataService : IFindProvinceDataService
    {
        private IOvMainDbContext _ovMainDbContext;
        public FindProvinceDataService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }

        public async Task<IEnumerable<PersistedProvince>> FindAsync(ProvinceFilter filter, CancellationToken cancellationToken)
        {
            var provinces = _ovMainDbContext.Provinces
                            .Include(p => p.AutonomousCommunity)
                            .Where(ac => true);

            if (filter.Id != default(int))
            {
                provinces = provinces.Where(p => p.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                provinces = provinces.Where(ac => ac.Name.Equals(filter.Name));
            }

            var provincesToReturn = await provinces.OrderBy(ac => ac.Name).ToListAsync(cancellationToken);

            if (!filter.AutonomousCommunityIncluded)
            {
                provincesToReturn.ForEach(p => p.AutonomousCommunity = null);
            }

            return provincesToReturn;
        }
    }
}
