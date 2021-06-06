using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.User.Find;
using OV.MainDb.User.Find.Models.Public;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.NotifyService.Service.User
{
    public interface IUserService
    {
        Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken);
    }
    public class UserService : IUserService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindUserService _findUserService;
        public UserService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            _findUserService = new FindUserService(new FindUserDataService(_ovMainDbContextFactory));
        }

        public async Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            return await _findUserService.FindAsync(filter, cancellationToken);
        }
    }
}
