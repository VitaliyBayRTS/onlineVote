using Microsoft.EntityFrameworkCore;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Find;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Election.Models;
using OV.MainDb.User.Models;
using OV.Models.MainDb.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.GetForVote
{
    public interface IGetElectionForVoteDataService
    {
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindForVoteAsync(int tblUser_UID, CancellationToken cancellationToken);
    }
    public class GetElectionForVoteDataService : IGetElectionForVoteDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindElectionDataService _findElectionDataService;
        public GetElectionForVoteDataService(IOvMainDbContextFactory ovMainDbContextFactory, IFindElectionDataService findElectionDataService)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
            _findElectionDataService = findElectionDataService ?? throw new ArgumentNullException(nameof(findElectionDataService));
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindForVoteAsync(int tblUser_UID, CancellationToken cancellationToken)
        {
            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var users = ovMainDbContext.Users
                            .Include(h => h.Province)
                            .Where(h => h.IsAutorized && h.Id == tblUser_UID);
            var usersList = await users.ToListAsync(cancellationToken);

            usersList.ForEach(u => u.Province.Users = null);

            var singleUser = usersList.First();

            var election = await _findElectionDataService.FindAsync(ElectionFilter.IncludType().AndACIncluded().AndProvinceIncluded().AndOrganizersIncluded(),
               cancellationToken);

            var matchedElections = election.Where(e => IsNationalLevel(e, singleUser) || IsACLevel(e, singleUser) || IsProvinceLevel(e, singleUser));

            var electionToReturn = matchedElections.ToList();

            return electionToReturn.Select(e => e.ToElection()).ToList();
        }









        private const string allowedLettersForDocument = "TRWAGMYFPDXBNJZSQVHLCKET";
        private bool IsNationalLevel(PersistedElection election, PersistedUser user)
        {
            if(election.Type.Code == OV_Types.NL.ToString() && GetDocumentType(user.DNI_NIE) == "DNI")
            {
                return true;
            }
            return false;
        }

        private bool IsACLevel(PersistedElection election, PersistedUser user)
        {
            if(election.Type.Code == OV_Types.ACL.ToString() && GetDocumentType(user.DNI_NIE) == "DNI" 
                && election.tblAutonomousCommunity_UID == user.Province.tblAutonomousCommunity_UID)
            {
                return true;
            }
            return false;
        }

        private bool IsProvinceLevel(PersistedElection election, PersistedUser user)
        {
            if(election.Type.Code == OV_Types.PL.ToString() && GetDocumentType(user.DNI_NIE) == "DNI" 
                && election.tblProvince_UID == user.TblProvince_UID)
            {
                return true;
            }
            return false;
        }











        private string GetDocumentType(string documentCode)
        {
            if (documentCode.Length == 9 && isValidDNI(documentCode))
            {
                return "DNI";
            }
            else if (documentCode.Length == 10 && isValidNIE(documentCode))
            {
                return "NIE";
            }
            return "Unknow";
        }

        private bool isValidNIE(string documentCode)
        {
            const int POSITION_OF_FIRST_LETTER = 1;
            const int NUMBER_OF_NON_NUMERIC_CHARACTERS_IN_NIE = 3;
            int LENGTH_OF_NUMBERS_IN_NIE = documentCode.Length - NUMBER_OF_NON_NUMERIC_CHARACTERS_IN_NIE;
            try
            {
                var NIEfirstLetter = documentCode.ToUpper().First();
                var NIEnumbers = Int32.Parse(documentCode.Substring(POSITION_OF_FIRST_LETTER, LENGTH_OF_NUMBERS_IN_NIE));
                var NIEletter = documentCode.ToUpper().Last();
                var numberValueOfFirstNIELetter = GetNumberOfFirstNIELetter(NIEfirstLetter);
                var positionOfCorrectLetter = (NIEnumbers + numberValueOfFirstNIELetter) % 23;
                return allowedLettersForDocument[positionOfCorrectLetter] == NIEletter;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int GetNumberOfFirstNIELetter(char letter)
        {
            if (letter == 'X')
            {
                return 0;
            }
            else if (letter == 'Y')
            {
                return 10000000;
            }
            else if (letter == 'Z')
            {
                return 20000000;
            }
            else
            {
                throw new Exception("Invalid first character");
            }
        }

        private bool isValidDNI(string documentCode)
        {
            try
            {
                var DNInumbers = Int32.Parse(documentCode.Remove(documentCode.Length - 1));
                var DNIletter = documentCode.ToUpper().Last();
                var positionOfCorrectLetter = DNInumbers % 23;
                return allowedLettersForDocument[positionOfCorrectLetter] == DNIletter;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
