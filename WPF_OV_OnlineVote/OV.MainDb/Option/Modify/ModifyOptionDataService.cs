using OV.MainDb.Configuration;
using OV.MainDb.Option.Models;
using OV.MainDb.Option.Modify.Models.Public;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Modify
{
    public interface IModifyOptionDataService
    {
        Task<PersistedOption> ModifyAsync(ModifyOptionCandidate candidate, CancellationToken cancellationToken);
    }
    public class ModifyOptionDataService : IModifyOptionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public ModifyOptionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<PersistedOption> ModifyAsync(ModifyOptionCandidate candidate, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var option = ovMainDbContext.Options.First(o => o.Id == candidate.Id);

            option.Name = candidate.Name;
            option.Description = candidate.Description;

            await ovMainDbContext.SaveChangesAsync(cancellationToken);

            return ovMainDbContext.Options.First(e => e.Id == candidate.Id);
        }
    }
}
