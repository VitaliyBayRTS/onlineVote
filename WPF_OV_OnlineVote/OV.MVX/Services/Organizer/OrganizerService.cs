using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.Organizer.Create;
using OV.MainDb.Organizer.Create.Models.Public;
using OV.MainDb.Organizer.Find;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MainDb.Organizer.Models.Public;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Organizer
{
    public interface IOrganizerService
    {
        Task<IEnumerable<OV.Models.MainDb.Organizer.Organizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken);
        Task<ICreateOrganizerResponse> CreateAsync(CandidateOrganizer candidate, CancellationToken cancellationToken);
        Task<bool> CreateRangeAsync(List<CandidateOrganizer> candidates, CancellationToken cancellationToken);
    }
    public class OrganizerService : IOrganizerService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindOrganizerService _findOrganizerService;
        private ICreateOrganizerService _createOrganizerService;
        public OrganizerService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();

            var findOrganizerDataService = new FindOrganizerDataService(_ovMainDbContextFactory);
            _findOrganizerService = new FindOrganizerService(findOrganizerDataService);

            var candidateOrganizerValidator = new CandidateOrganizerValidator();
            _createOrganizerService = new CreateOrganizerService(new CreateOrganizerDataService(_ovMainDbContextFactory), candidateOrganizerValidator);
        }

        public async Task<ICreateOrganizerResponse> CreateAsync(CandidateOrganizer candidate, CancellationToken cancellationToken)
        {
            return await _createOrganizerService.CreateAsync(candidate, cancellationToken);
        }

        public async Task<bool> CreateRangeAsync(List<CandidateOrganizer> candidates, CancellationToken cancellationToken)
        {
            return await _createOrganizerService.CreateRangeAsync(candidates, cancellationToken);
        }

        public Task<IEnumerable<OV.Models.MainDb.Organizer.Organizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken)
        {
            return _findOrganizerService.FindAsync(filter, cancellationToken);
        }
    }
}
