using OV.MainDb.Habitant.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Habitant.Find
{
    public interface IFindHabitantService
    {
        Task<IEnumerable<OV.Models.MainDb.Habitant.Habitant>> FindAsync(HabitantFilter filter, CancellationToken cancellationToken);
    }
    public class FindHabitantService : IFindHabitantService
    {
        private readonly IFindHabitantDataService _findHabitantDataService;

        public FindHabitantService(IFindHabitantDataService findHabitantDataService)
        {
            _findHabitantDataService = findHabitantDataService ??
                                                  throw new ArgumentNullException(nameof(findHabitantDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Habitant.Habitant>> FindAsync(HabitantFilter filter, CancellationToken cancellationToken)
        {
            var habitants = await _findHabitantDataService.FindAsync(filter, cancellationToken);
            return habitants.Select(h => h.ToHabitant());
        }
    }
}
