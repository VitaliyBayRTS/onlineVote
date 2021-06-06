using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Find;
using OV.MainDb.Result.GetResult;
using OV.MainDb.Result.GetResult.Models.Public;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Result
{
    public interface IResultService
    {
        Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken);
    }
    public class ResultService : IResultService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IGetResultService _getResultService;

        public ResultService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var getResultDataService = new GetResultDataService(ovMainDbContext);
            _getResultService = new GetResultService(getResultDataService, new FindOptionDataService(_ovMainDbContextFactory));
        }

        public async Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken)
        {
            return await _getResultService.GetResult(tblElection_UID, cancellationToken);
        }
    }
}
