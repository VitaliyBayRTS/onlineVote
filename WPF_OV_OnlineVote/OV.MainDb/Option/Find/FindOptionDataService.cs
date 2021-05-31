using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Option.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Find
{
    public interface IFindOptionDataService
    {
        Task<IEnumerable<PersistedOption>> FindAsync(OptionFilter filter, CancellationToken cancellationToken);
    }
    public class FindOptionDataService : IFindOptionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindOptionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedOption>> FindAsync(OptionFilter filter, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var options = ovMainDbContext.Options
                            .Where(o => true);

            if(filter.Id.HasValue)
            {
                options = options.Where(o => o.Id == filter.Id);
            }

            if(filter.ElectionId.HasValue)
            {
                options = options.Where(o => o.tblElection_UID == filter.ElectionId);
            }

            var optionsToReturn = await options.ToListAsync();

            return optionsToReturn;
        }
    }
}
