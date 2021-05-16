using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Habitant.Find.Models.Public;
using OV.MainDb.Habitant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Habitant.Find
{
    public interface IFindHabitantDataService
    {
        Task<IEnumerable<PersistedHabitant>> FindAsync(HabitantFilter filter, CancellationToken cancellationToken);
    }

    public class FindHabitantDataService : IFindHabitantDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindHabitantDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedHabitant>> FindAsync(HabitantFilter filter, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var habitants = ovMainDbContext.Habitants
                            .Include(h => h.User)
                            .Where(h => true);


            if(filter.Id != default(int))
            {
                habitants = habitants.Where(h => h.Id == filter.Id);
            }

            if(!string.IsNullOrEmpty(filter.DNI_NIE))
            {
                habitants = habitants.Where(h => h.User.DNI_NIE.Equals(filter.DNI_NIE));
            }

            var habitantsToReturn = await habitants.ToListAsync(cancellationToken);

            if(!filter.UserIncluded)
            {
                habitantsToReturn.ForEach(h => h.User = null);
            }

            return habitantsToReturn;
        }
    }
}
