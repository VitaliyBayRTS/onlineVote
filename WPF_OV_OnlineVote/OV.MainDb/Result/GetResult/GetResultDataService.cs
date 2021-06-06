using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Result.GetResult.Models.Public;
using OV.Models.MainDb.Type;
using OV.Services.DocumentValidator;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Result.GetResult
{
    public interface IGetResultDataService
    {
        Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken);
    }
    public class GetResultDataService : IGetResultDataService
    {
        private IOvMainDbContext _ovMainDbContext;
        public GetResultDataService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }

        public async Task<GetResultResponse> GetResult(int tblElection_UID, CancellationToken cancellationToken)
        {
            var totalHabitantCount = 0;
            var election = _ovMainDbContext.Elections.Include(e => e.Type).First(e => e.Id == tblElection_UID);

            if(election.Type.Code == OV_Types.NL.ToString())
            {
                totalHabitantCount = _ovMainDbContext.Users.Where(u => DocumentValidation.GetDocumentType(u.DNI_NIE) == "DNI").ToList().Count;
            }

            if(election.Type.Code == OV_Types.ACL.ToString())
            {
                totalHabitantCount = _ovMainDbContext.Users.Include(u => u.Province).Where(u => u.Province.tblAutonomousCommunity_UID == election.tblAutonomousCommunity_UID).ToList().Count;
            }

            if(election.Type.Code == OV_Types.PL.ToString())
            {
                totalHabitantCount = _ovMainDbContext.Users.Where(u => u.TblProvince_UID == election.tblProvince_UID).ToList().Count;
            }

            var habitantCountThatParticipateResult = _ovMainDbContext.UserElections.Where(ue => ue.TblElection_UID == election.Id);
            var habitantCountThatParticipateList = await habitantCountThatParticipateResult.ToListAsync(cancellationToken);

            return new GetResultResponse()
            {
                TblElection_UID = tblElection_UID,
                TotalHabitant = totalHabitantCount,
                HabitantCountThatParticipate = habitantCountThatParticipateList.Count
            };
        }
    }
}
