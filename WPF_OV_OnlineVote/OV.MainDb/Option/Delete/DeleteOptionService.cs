using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Delete
{
    public interface IDeleteOptionService
    {
        Task<bool> DeleteAsync(int optionId, CancellationToken cancellationToken);
    }
    public class DeleteOptionService : IDeleteOptionService
    {
        private IDeleteOptionDataService _deleteOptionDataService;

        public DeleteOptionService(IDeleteOptionDataService deleteoptionDataService)
        {
            _deleteOptionDataService = deleteoptionDataService ?? throw new ArgumentNullException(nameof(deleteoptionDataService));
        }

        public async Task<bool> DeleteAsync(int optionId, CancellationToken cancellationToken)
        {
            try
            {
                return await _deleteOptionDataService.DeleteAsync(optionId, cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
