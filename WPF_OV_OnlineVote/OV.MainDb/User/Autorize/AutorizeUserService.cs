using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Autorize
{
    public interface IAutorizeUserService
    {
        Task<bool> AutorizeAsync(int userId, CancellationToken cancellationToken);
    }

    public class AutorizeUserService : IAutorizeUserService
    {
        private IAutorizeUserDataService _autorizeUserDataService;

        public AutorizeUserService(IAutorizeUserDataService autorizeUserDataService)
        {
            _autorizeUserDataService = autorizeUserDataService ?? throw new ArgumentNullException(nameof(autorizeUserDataService));
        }

        public async Task<bool> AutorizeAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _autorizeUserDataService.AutorizeAsync(userId, cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
