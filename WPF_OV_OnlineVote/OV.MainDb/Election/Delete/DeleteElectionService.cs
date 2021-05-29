using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Delete
{
    public interface IDeleteElectionService
    {
        Task<bool> DeleteAsync(int electionId, CancellationToken cancellationToken);
    }
    public class DeleteElectionService : IDeleteElectionService
    {
        private IDeleteElectionDataService _deleteElectionDataService;
        public DeleteElectionService(IDeleteElectionDataService deleteElectionDataService)
        {
            _deleteElectionDataService = deleteElectionDataService ?? throw new ArgumentNullException(nameof(deleteElectionDataService));
        }

        public async Task<bool> DeleteAsync(int electionId, CancellationToken cancellationToken)
        {
            try
            {
                return await _deleteElectionDataService.DeleteAsync(electionId, cancellationToken);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
