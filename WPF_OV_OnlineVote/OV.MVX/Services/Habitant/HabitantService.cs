using OV.MainDb.Configuration;
using OV.MainDb.Habitant.Find;
using OV.MainDb.Habitant.Find.Models.Public;
using OV.MVX.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Habitant
{
    public interface IHabitantService
    {
        Task<IEnumerable<OV.Models.MainDb.Habitant.Habitant>> FindAsync(HabitantFilter filter, CancellationToken cancellationToken);
    }
    public class HabitantService : IHabitantService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindHabitantService _findHabitantService;
        public HabitantService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            var findHabitantDataService = new FindHabitantDataService(_ovMainDbContextFactory);
            _findHabitantService = new FindHabitantService(findHabitantDataService);
        }

        public async Task<IEnumerable<OV.Models.MainDb.Habitant.Habitant>> FindAsync(HabitantFilter filter, CancellationToken cancellationToken)
        {
            return await _findHabitantService.FindAsync(filter, cancellationToken);
        }
    }
}
