using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Delete
{
    public interface IDeleteOptionDataService
    {
        Task<bool> DeleteAsync(int optionId, CancellationToken cancellationToken);
    }
    public class DeleteOptionDataService : IDeleteOptionDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public DeleteOptionDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<bool> DeleteAsync(int optionId, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var option = ovMainDbContext.Options.First(o => o.Id == optionId);

            ovMainDbContext.Options.Remove(option);

            await ovMainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
