using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option;
using OV.MainDb.Option.IncreaseVotes;
using OV.MainDb.Option.IncreaseVotes.Models.Public;
using OV.MainDb.Option.Models;
using OV.MainDb.UserElection.Find;
using OV.MainDb.UserElection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Option.IncreaseVote
{
    public class IncreaseVoteServiceTests
    {
        static PersistedOption _dummyOption1 = new PersistedOption()
        {
            Id = 1,
            tblElection_UID = 1,
            Name = "Option1",
            Description = "Desc1"
        };
        public class TestForIncreaseVotes
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IIncreaseVotesService _increaseVotesService;
            private readonly CancellationToken cancellationToken = default;

            public TestForIncreaseVotes()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();

                IIncreaseVotesValidator validator = new IncreaseVotesValidator(new FindUserElectionDataService(_inMemoryOvMainDbContextFactory));
                _increaseVotesService = new IncreaseVotesService(new IncreaseVotesDataService(_inMemoryOvMainDbContextFactory.Create()), validator);

                _inMemoryOvMainDbContext.Options.Add(_dummyOption1);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
            }

            [Fact]
            public async void ShoudlIncreaseVotes()
            {
                //Arrange
                var optionToModify = _dummyOption1;
                var optionId = optionToModify.Id.Value;

                IncreaseVotesRequest request = new IncreaseVotesRequest()
                {
                    TblUser_UID = 1,
                    TblElection_UID = 1,
                    TblOption_UID = optionId
                };

                //Act
                var result = await _increaseVotesService.IncreaseVote(request, cancellationToken);

                //Assert
                var isSuccess = result is IncreaseVotesSuccess;
                isSuccess.Should().BeTrue();
                _inMemoryOvMainDbContext.UserElections
                    .First(ue => ue.TblElection_UID == request.TblElection_UID && ue.TblUser_UID == request.TblUser_UID)
                    .Should().NotBeNull();
                _inMemoryOvMainDbContext.Options.First(o => o.Id == optionId).Votes.Should().Be(optionToModify.Votes + 1);
            }

            [Fact]
            public async void ShoudlNotIncreaseVotesIfTblOption_UIDIsEmpty()
            {
                //Arrange
                var optionToModify = _dummyOption1;
                var optionId = optionToModify.Id.Value;

                IncreaseVotesRequest request = new IncreaseVotesRequest()
                {
                    TblUser_UID = 1,
                    TblElection_UID = 1
                };

                //Act
                var result = await _increaseVotesService.IncreaseVote(request, cancellationToken);

                //Assert
                if (result is IncreaseVotesFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.TblOption_UIDIsEmpty.ToString());
                }
                _inMemoryOvMainDbContext.UserElections
                    .FirstOrDefault(ue => ue.TblElection_UID == request.TblElection_UID && ue.TblUser_UID == request.TblUser_UID)
                    .Should().BeNull();
                _inMemoryOvMainDbContext.Options.First(o => o.Id == optionId).Votes.Should().Be(optionToModify.Votes);
            }

            [Fact]
            public async void ShoudlNotIncreaseVotesIfTblUser_UIDIsEmpty()
            {
                //Arrange
                var optionToModify = _dummyOption1;
                var optionId = optionToModify.Id.Value;

                IncreaseVotesRequest request = new IncreaseVotesRequest()
                {
                    TblElection_UID = 1,
                    TblOption_UID = optionId
                };

                //Act
                var result = await _increaseVotesService.IncreaseVote(request, cancellationToken);

                //Assert
                if (result is IncreaseVotesFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.TblUser_UIDIsEmpty.ToString());
                }
                _inMemoryOvMainDbContext.Options.First(o => o.Id == optionId).Votes.Should().Be(optionToModify.Votes);
            }

            [Fact]
            public async void ShoudlNotIncreaseVotesIfTblElection_UIDIsEmpty()
            {
                //Arrange
                var optionToModify = _dummyOption1;
                var optionId = optionToModify.Id.Value;

                IncreaseVotesRequest request = new IncreaseVotesRequest()
                {
                    TblUser_UID = 1,
                    TblOption_UID = optionId
                };

                //Act
                var result = await _increaseVotesService.IncreaseVote(request, cancellationToken);

                //Assert
                if (result is IncreaseVotesFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.TblElection_UIDIsEmpty.ToString());
                }
                _inMemoryOvMainDbContext.Options.First(o => o.Id == optionId).Votes.Should().Be(optionToModify.Votes);
            }

            [Fact]
            public async void ShoudlNotIncreaseVotesIfUserAlreadyVoted()
            {
                //Arrange
                var optionToModify = _dummyOption1;
                var optionId = optionToModify.Id.Value;
                PersistedUserElection persistedUserElection = new PersistedUserElection()
                {
                    TblUser_UID = 1,
                    TblElection_UID = 1
                };
                _inMemoryOvMainDbContext.UserElections.Add(persistedUserElection);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                IncreaseVotesRequest request = new IncreaseVotesRequest()
                {
                    TblUser_UID = persistedUserElection.TblUser_UID,
                    TblElection_UID = persistedUserElection.TblElection_UID,
                    TblOption_UID = optionId
                };

                //Act
                var result = await _increaseVotesService.IncreaseVote(request, cancellationToken);

                //Assert
                if (result is IncreaseVotesFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.RelationUserElectionAlreadyExists.ToString());
                }
                _inMemoryOvMainDbContext.Options.First(o => o.Id == optionId).Votes.Should().Be(optionToModify.Votes);
            }
        }
    }
}
