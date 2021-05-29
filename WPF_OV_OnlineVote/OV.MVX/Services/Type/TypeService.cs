using OV.MainDb.Configuration;
using OV.MainDb.Type.Find;
using OV.MainDb.Type.Find.Models.Public;
using OV.MVX.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Type
{
    public interface ITypeService
    {
        Task<IEnumerable<OV.Models.MainDb.Type.TypeObject>> FindAsync(TypeFilter filter, CancellationToken cancellationToken);
    }
    public class TypeService : ITypeService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindTypeService _findTypeService;
        public TypeService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            _findTypeService = new FindTypeService(new FindTypeDataService(_ovMainDbContextFactory));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Type.TypeObject>> FindAsync(TypeFilter filter, CancellationToken cancellationToken)
        {
            return await _findTypeService.FindAsync(filter, cancellationToken);
        }
    }
}
