using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.SuperAdmin.Find.Models.Public;
using OV.MainDb.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.SuperAdmin.Find
{
    public interface IFindSuperAdminDataService
    {
        Task<IEnumerable<PersistedSuperAdmin>> FindAsync(SuperAdminFilter filter, CancellationToken cancellationToken);
    }
    public class FindSuperAdminDataService : IFindSuperAdminDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public FindSuperAdminDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<IEnumerable<PersistedSuperAdmin>> FindAsync(SuperAdminFilter filter, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var superAdmin = ovMainDbContext.SuperAdmin
                            .Include(h => h.User)
                            .Where(h => h.User.IsAutorized);


            if (filter.Id != default(int))
            {
                superAdmin = superAdmin.Where(h => h.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.DNI_NIE) && !string.IsNullOrEmpty(filter.Password) && !string.IsNullOrEmpty(filter.ReferenceNumber))
            {
                superAdmin = superAdmin.Where(h => h.User.DNI_NIE.Equals(filter.DNI_NIE)
                            && h.User.Password.Equals(filter.Password)
                            && h.ReferenceNumber.Equals(filter.ReferenceNumber));
            }

            if (!string.IsNullOrEmpty(filter.DNI_NIE) && string.IsNullOrEmpty(filter.Password) && string.IsNullOrEmpty(filter.ReferenceNumber))
            {
                superAdmin = superAdmin.Where(h => h.User.DNI_NIE.Equals(filter.DNI_NIE));
            }

            var superAdminsToReturn = await superAdmin.ToListAsync(cancellationToken);

            if (!filter.UserIncluded)
            {
                superAdminsToReturn.ForEach(h => h.User = null);
            }

            return superAdminsToReturn;
        }
    }
}
