using OV.MainDb.User.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Find
{
    public interface IFindUserService
    {
        Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken);
    }
    public class FindUserService : IFindUserService
    {
        private readonly IFindUserDataService _findUserDataService;

        public FindUserService(IFindUserDataService findUserDataService)
        {
            _findUserDataService = findUserDataService ??
                                                  throw new ArgumentNullException(nameof(findUserDataService));
        }
        public async Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            var users = await _findUserDataService.FindAsync(filter, cancellationToken);
            return users.Select(u => u.ToUser());
        }
    }
}
