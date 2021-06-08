using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Find;
using OV.MainDb.Result.Create;
using OV.MainDb.Result.Create.Models.Public;
using OV.MainDb.Result.GetResult;
using OV.MainDb.Result.GetResult.Models.Public;
using System.Threading;
using System.Threading.Tasks;

namespace OV.NotifyService.Result
{
    public interface IResultService
    {
        Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken);
        Task<bool> CreateAsync(CreateResultRequest request, CancellationToken cancellationToken);
    }
    public class ResultService : IResultService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IGetResultService _getResultService;
        private ICreateResultService _createResultService;

        public ResultService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var getResultDataService = new GetResultDataService(ovMainDbContext);
            _getResultService = new GetResultService(getResultDataService, new FindOptionDataService(_ovMainDbContextFactory));

            _createResultService = new CreateResultService(new CreateRusultDataService(_ovMainDbContextFactory));
        }

        public Task<bool> CreateAsync(CreateResultRequest request, CancellationToken cancellationToken)
        {
            return _createResultService.CreateAsync(request, cancellationToken);
        }

        public async Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken)
        {
            return await _getResultService.GetResult(tblElection_UID, cancellationToken);
        }
    }
}
