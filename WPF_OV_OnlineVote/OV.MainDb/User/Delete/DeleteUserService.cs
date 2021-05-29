using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Delete
{
    public interface IDeleteUserService
    {
        Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken);
    }
    public class DeleteUserService : IDeleteUserService
    {
        private IDeleteUserDataService _deleteUserDataService;

        public DeleteUserService(IDeleteUserDataService deleteUserDataService)
        {
            _deleteUserDataService = deleteUserDataService ?? throw new ArgumentNullException(nameof(deleteUserDataService));
        }

        public async Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _deleteUserDataService.DeleteAsync(userId, cancellationToken);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
