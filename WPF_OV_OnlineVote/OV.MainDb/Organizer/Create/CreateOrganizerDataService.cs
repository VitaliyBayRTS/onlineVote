using OV.MainDb.Configuration;
using OV.MainDb.Organizer.Models;
using OV.MainDb.Organizer.Models.Public;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Organizer.Create
{
    public interface ICreateOrganizerDataService
    {
        Task<PersistedOrganizer> CreateAsync(CandidateOrganizer candidate, CancellationToken cancellationToken);
        Task<bool> CreateRangeAsync(List<CandidateOrganizer> candidates, CancellationToken cancellationToken);
    }
    public class CreateOrganizerDataService : ICreateOrganizerDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public CreateOrganizerDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<PersistedOrganizer> CreateAsync(CandidateOrganizer candidate, CancellationToken cancellationToken)
        {
            var ovmainDbContext = _ovMainDbContextFactory.Create();

            var organizer = new PersistedOrganizer(candidate);

            var newOrganizer = ovmainDbContext.Organizers.Add(organizer);

            await ovmainDbContext.SaveChangesAsync(cancellationToken);

            return newOrganizer.Entity;
        }

        public async Task<bool> CreateRangeAsync(List<CandidateOrganizer> candidates, CancellationToken cancellationToken)
        {
            var ovmainDbContext = _ovMainDbContextFactory.Create();

            List<PersistedOrganizer> organizers = new List<PersistedOrganizer>();
            foreach (var candidate in candidates)
            {
                organizers.Add(new PersistedOrganizer(candidate));
            }

           ovmainDbContext.Organizers.AddRange(organizers);

            await ovmainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
