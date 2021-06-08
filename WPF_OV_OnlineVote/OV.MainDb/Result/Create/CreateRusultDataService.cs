using OV.MainDb.Configuration;
using OV.MainDb.Result.Create.Models.Public;
using OV.MainDb.Result.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Result.Create
{
    public interface ICreateRusultDataService
    {
        Task<bool> CreateAsync(CreateResultRequest request, CancellationToken cancellationToken);
    }
    public class CreateRusultDataService : ICreateRusultDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public CreateRusultDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<bool> CreateAsync(CreateResultRequest request, CancellationToken cancellationToken)
        {
            var ovmainDbContext = _ovMainDbContextFactory.Create();

            PersistedResult result = new PersistedResult()
            {
                TotalVotes = request.TotalVotes,
                TotalHabitants = request.TotalHabitants,
                HabitantsThatParticipate = request.TotalHabitantsThatParticipate,
                Winner_UID = request.Winner_UID,
                TblElection_UID = request.TblElection_UID
            };

            ovmainDbContext.Results.Add(result);

            await ovmainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
