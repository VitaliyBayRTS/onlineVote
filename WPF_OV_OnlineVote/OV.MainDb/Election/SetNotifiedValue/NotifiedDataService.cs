using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.SetNotifiedValue
{
    public interface INotifiedDataService
    {
        Task Notify(int tblElection_UID, CancellationToken cancellationToken);
    }
    public class NotifiedDataService : INotifiedDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public NotifiedDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task Notify(int tblElection_UID, CancellationToken cancellationToken)
        {
            var ovmainDbContext = _ovMainDbContextFactory.Create();

            var election = ovmainDbContext.Elections.FirstOrDefault(e => e.Id == tblElection_UID);

            election.IsNotified = true;

            await ovmainDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
