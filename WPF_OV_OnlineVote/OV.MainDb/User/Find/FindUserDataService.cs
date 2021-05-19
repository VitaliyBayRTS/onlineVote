using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.User.Find.Models.Public;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Find
{
    public interface IFindUserDataService
    {
        Task<IEnumerable<PersistedUser>> FindAsync(UserFilter filter, CancellationToken cancellationToken);
    }
    public class FindUserDataService : IFindUserDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindUserDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedUser>> FindAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();
            var users = ovMainDbContext.Users
                        .Where(u => true);

            if(filter.Id != default(int))
            {
                users = users.Where(u => u.Id == filter.Id);
            }

            if(filter.Unautorized)
            {
                users = users.Where(u => !u.IsAutorized);
            }

            return await users.ToListAsync(cancellationToken);

        }
    }
}
