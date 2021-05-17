using OV.MainDb.Habitant.Find;
using OV.MainDb.Organizer.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Organizer.Find
{
    public interface IFindOrganizerService
    {
        Task<IEnumerable<OV.Models.MainDb.Organizer.Organizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken);
    }
    public class FindOrganizerService : IFindOrganizerService
    {
        private readonly IFindOrganizerDataService _findOrganizerDataService;

        public FindOrganizerService(IFindOrganizerDataService findOrganizerDataService)
        {
            _findOrganizerDataService = findOrganizerDataService ??
                                                  throw new ArgumentNullException(nameof(findOrganizerDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Organizer.Organizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken)
        {
            var organizers = await _findOrganizerDataService.FindAsync(filter, cancellationToken);
            return organizers.Select(o => o.ToOrganizer());
        }
    }
}
