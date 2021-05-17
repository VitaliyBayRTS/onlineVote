using OV.MainDb.Configuration;
using OV.MainDb.Organizer.Find;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MVX.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Organizer
{
    public interface IOrganizerService
    {
        Task<IEnumerable<OV.Models.MainDb.Organizer.Organizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken);
    }
    public class OrganizerService : IOrganizerService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindOrganizerService _findOrganizerService;
        public OrganizerService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();

            var findOrganizerDataService = new FindOrganizerDataService(_ovMainDbContextFactory);
            _findOrganizerService = new FindOrganizerService(findOrganizerDataService);
        }

        public Task<IEnumerable<OV.Models.MainDb.Organizer.Organizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken)
        {
            return _findOrganizerService.FindAsync(filter, cancellationToken);
        }
    }
}
