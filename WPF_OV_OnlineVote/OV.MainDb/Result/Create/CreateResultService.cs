using OV.MainDb.Result.Create.Models.Public;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Result.Create
{
    public interface ICreateResultService
    {
        Task<bool> CreateAsync(CreateResultRequest request, CancellationToken cancellationToken);
    }
    public class CreateResultService : ICreateResultService
    {

        private ICreateRusultDataService _createRusultDataService;

        public CreateResultService(ICreateRusultDataService createRusultDataService)
        {
            _createRusultDataService = createRusultDataService ?? throw new ArgumentNullException(nameof(createRusultDataService));
        }

        public async Task<bool> CreateAsync(CreateResultRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _createRusultDataService.CreateAsync(request, cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
