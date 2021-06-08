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
                        .Include(u => u.Province)
                        .ThenInclude(p => p.AutonomousCommunity)
                        .Where(u => true);

            if(filter.Id != default(int))
            {
                users = users.Where(u => u.Id == filter.Id);
            }

            if(!string.IsNullOrEmpty(filter.DNI_NIE))
            {
                users = users.Where(u => u.DNI_NIE == filter.DNI_NIE);
            }

            if(filter.Ac != default(int))
            {
                users = users.Where(u => u.Province.tblAutonomousCommunity_UID == filter.Ac);
            }

            if(filter.Province != default(int))
            {
                users = users.Where(u => u.TblProvince_UID == filter.Province);
            }

            if(filter.Unautorized)
            {
                users = users.Where(u => !u.IsAutorized);
            }

            if(filter.Autorized)
            {
                users = users.Where(u => u.IsAutorized);
            }

            var usersToReturn = await users.ToListAsync(cancellationToken);

            if(!filter.IncludeAC)
            {
                usersToReturn.ForEach(u => u.Province.AutonomousCommunity = null);
            }

            if (!filter.IncludeProvince)
            {
                usersToReturn.ForEach(u => u.Province = null);
            } else
            {
                usersToReturn.ForEach(u => u.Province.Users = null);
            }


            return usersToReturn;

        }
    }
}
