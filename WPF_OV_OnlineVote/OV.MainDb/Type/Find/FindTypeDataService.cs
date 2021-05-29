using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Type.Find.Models.Public;
using OV.MainDb.Type.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Type.Find
{
    public interface IFindTypeDataService
    {
        Task<IEnumerable<PersistedType>> FindAsync(TypeFilter filter, CancellationToken cancellationToken);
    }

    public class FindTypeDataService : IFindTypeDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindTypeDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedType>> FindAsync(TypeFilter filter, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var types = ovMainDbContext.Types.Where(t => true);

            if(!string.IsNullOrEmpty(filter.Code))
            {
                types = types.Where(t => string.Equals(t.Code, filter.Code));
            }


            return await types.ToListAsync(cancellationToken);
        }
    }
}
