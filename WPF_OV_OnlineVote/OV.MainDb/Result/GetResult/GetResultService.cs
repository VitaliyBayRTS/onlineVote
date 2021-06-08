using OV.MainDb.Option.Find;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Result.GetResult.Models.Public;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Result.GetResult
{
    public interface IGetResultService
    {
        Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken);
    }
    public class GetResultService : IGetResultService
    {
        private readonly IGetResultDataService _getResultDataService;
        private readonly IFindOptionDataService _findOptionDataService;

        public GetResultService(IGetResultDataService getResultDataService, IFindOptionDataService findOptionDataService)
        {
            _getResultDataService = getResultDataService ??
                                                  throw new ArgumentNullException(nameof(getResultDataService));
            _findOptionDataService = findOptionDataService ??
                                                  throw new ArgumentNullException(nameof(findOptionDataService));
        }

        public async Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _getResultDataService.GetResult(tblElection_UID, cancellationToken);

                var optionsResponse = await _findOptionDataService.FindAsync(OptionFilter.ByElectionId(tblElection_UID), cancellationToken);

                response.Options = optionsResponse.Select(o => o.ToOption()).ToList();

                return response;
            }
            catch (Exception e)
            {
                return new GetResultResponse()
                {
                    Error = true,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
