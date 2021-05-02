using Microsoft.EntityFrameworkCore;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OV.MainDb.AutonomousCommunity.Find
{
    public interface IFindAutonomousCommunityService
    {
        IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity> Find();
    }
    public class FindAutonomousCommunityService : IFindAutonomousCommunityService
    {
        private IOvMainDbContext _ovMainDbContext;
        public FindAutonomousCommunityService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }
        public IEnumerable<OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity> Find()
        {
            return _ovMainDbContext.AutonomousCommunities
                            .Include(ac => ac.Provinces)
                            .Select(ac => ac.ToAutonomousCommunity());
        }
    }
}
