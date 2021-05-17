using OV.MainDb.SuperAdmin.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.SuperAdmin.Find
{
    public interface IFindSuperAdminService
    {
        Task<IEnumerable<OV.Models.MainDb.SuperAdmin.SuperAdmin>> FindAsync(SuperAdminFilter filter, CancellationToken cancellationToken);
    }
    public class FindSuperAdminService : IFindSuperAdminService
    {
        private readonly IFindSuperAdminDataService _findSuperAdminDataService;

        public FindSuperAdminService(IFindSuperAdminDataService findSuperAdminDataService)
        {
            _findSuperAdminDataService = findSuperAdminDataService ??
                                                  throw new ArgumentNullException(nameof(findSuperAdminDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.SuperAdmin.SuperAdmin>> FindAsync(SuperAdminFilter filter, CancellationToken cancellationToken)
        {
            var superAdmins = await _findSuperAdminDataService.FindAsync(filter, cancellationToken);
            return superAdmins.Select(sa => sa.ToSuperAdmin());
        }
    }
}
