using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Autorize
{
    public interface IAutorizeUserDataService
    {
        Task<bool> AutorizeAsync(int userId, CancellationToken cancellationToken);
    }
    public class AutorizeUserDataService : IAutorizeUserDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public AutorizeUserDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<bool> AutorizeAsync(int userId, CancellationToken cancellationToken)
        {

            var _ovMainDbContext = _ovMainDbContextFactory.Create();

            var user = _ovMainDbContext.Users.First(u => u.Id == userId);

            user.IsAutorized = true;

            await _ovMainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
