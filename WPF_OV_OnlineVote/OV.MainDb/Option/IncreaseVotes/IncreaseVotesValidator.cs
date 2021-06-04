using FluentValidation;
using OV.MainDb.Option.IncreaseVotes.Models;
using OV.MainDb.UserElection.Find;
using System;
using System.Linq;

namespace OV.MainDb.Option.IncreaseVotes
{
    public interface IIncreaseVotesValidator : IIncreaseVotesValidatorBase
    {

    }

    public class IncreaseVotesValidator : IncreaseVotesValidatorBase, IIncreaseVotesValidator
    {
        public IncreaseVotesValidator(IFindUserElectionDataService findUserElectionDataService)
        {
            if (findUserElectionDataService == null) throw new ArgumentNullException(nameof(findUserElectionDataService));

            RuleFor(candidate => candidate.TblOption_UID)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.TblOption_UIDIsEmpty.ToString());

            RuleFor(candidate => candidate.TblElection_UID)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.TblElection_UIDIsEmpty.ToString());

            RuleFor(candidate => candidate.TblUser_UID)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.TblUser_UIDIsEmpty.ToString());


            RuleFor(candidate => new { candidate.TblElection_UID, candidate.TblUser_UID })
                .MustAsync(async (candidate, cancellationToken) =>
                {
                    var existingProvince = await findUserElectionDataService
                                    .FindAsync(candidate.TblUser_UID, candidate.TblElection_UID, cancellationToken);
                    return !existingProvince.Any();
                })
                .WithErrorCode(OptionFailureReason.RelationUserElectionAlreadyExists.ToString());
        }
    }
}
