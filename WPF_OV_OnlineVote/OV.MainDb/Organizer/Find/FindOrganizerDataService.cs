using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MainDb.Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Organizer.Find
{
    public interface IFindOrganizerDataService
    {
        Task<IEnumerable<PersistedOrganizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken);
    }
    public class FindOrganizerDataService : IFindOrganizerDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindOrganizerDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedOrganizer>> FindAsync(OrganizerFilter filter, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var organizers = ovMainDbContext.Organizers
                            .Include(h => h.User)
                            .Where(h => h.User.IsAutorized);


            if (filter.Id != default(int))
            {
                organizers = organizers.Where(h => h.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.DNI_NIE) && !string.IsNullOrEmpty(filter.Password) && !string.IsNullOrEmpty(filter.ReferenceNumber))
            {
                organizers = organizers.Where(h => h.User.DNI_NIE.Equals(filter.DNI_NIE) 
                            && h.User.Password.Equals(filter.Password)
                            && h.ReferenceNumber.Equals(filter.ReferenceNumber));
            }

            if (!string.IsNullOrEmpty(filter.DNI_NIE) && string.IsNullOrEmpty(filter.Password) && string.IsNullOrEmpty(filter.ReferenceNumber))
            {
                organizers = organizers.Where(h => h.User.DNI_NIE.Equals(filter.DNI_NIE));
            }

            var organizersToReturn = await organizers.ToListAsync(cancellationToken);

            if (!filter.UserIncluded)
            {
                organizersToReturn.ForEach(h => h.User = null);
            }

            return organizersToReturn;
        }
    }
}
