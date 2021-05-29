using OV.MainDb.Type.Find.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Type.Find
{
    public interface IFindTypeService
    {
        Task<IEnumerable<OV.Models.MainDb.Type.TypeObject>> FindAsync(TypeFilter filter, CancellationToken cancellationToken);
    }

    public class FindTypeService : IFindTypeService
    {
        private readonly IFindTypeDataService _findTypeDataService;

        public FindTypeService(IFindTypeDataService findTypeDataService)
        {
            _findTypeDataService = findTypeDataService ??
                                                  throw new ArgumentNullException(nameof(findTypeDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Type.TypeObject>> FindAsync(TypeFilter filter, CancellationToken cancellationToken)
        {
            var types = await _findTypeDataService.FindAsync(filter, cancellationToken);
            return types.Select(t => t.ToType());
        }
    }
}
