using OV.MainDb.Configuration;
using OV.MainDb.Option.Models;
using OV.MainDb.Option.Models.Public;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Create
{
    public interface ICreateOptionDataService
    {
        Task<PersistedOption> CreateAsync(CandidateOption candidate, CancellationToken cancellationToken); 
    }
    public class CreateOptionDataService : ICreateOptionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public CreateOptionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<PersistedOption> CreateAsync(CandidateOption candidate, CancellationToken cancellationToken)
        {
            var ovmainDbContext = _ovMainDbContextFactory.Create();

            PersistedOption persistedOption = new PersistedOption()
            {
                Name = candidate.Name,
                Description = candidate.Description,
                tblElection_UID = candidate.tblElection_UID
            };

            var newOption = ovmainDbContext.Options.Add(persistedOption);

            await ovmainDbContext.SaveChangesAsync(cancellationToken);

            return newOption.Entity;
        }
    }
}
