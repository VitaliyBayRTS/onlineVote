using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election;
using OV.MainDb.Election.Models;
using OV.MainDb.Election.Modify;
using OV.MainDb.Election.Modify.Models.Public;
using OV.MainDb.Tests.Election.Find;
using System;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.Modify
{
    public class ModifyElectionServiceTests
    {
        public class TestForModifyElectionService
        {
            private PersistedElection electionToModify;

            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly IModifyElectionDataService _modifyElectionDataService;
            private readonly IModifyElectionValidator _candidateElectionValidator;
            private readonly IModifyElectionService _modifyElectionService;

            private readonly CancellationToken cancellationToken = default;

            public TestForModifyElectionService()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();


                _candidateElectionValidator = new ModifyElectionValidator();
                _modifyElectionDataService = new ModifyElectionDataService(_inMemoryOvMainDbContextFactory);

                _modifyElectionService = new ModifyElectionService(_modifyElectionDataService, _candidateElectionValidator);


                electionToModify = FindElectionPersistedGenerator._dummyPersistedElection1;
                _inMemoryOvMainDbContext.Elections.Add(electionToModify);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
            }


            [Fact]
            public async void ShouldCreateElection()
            {
                //Arrange
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = electionToModify.Id.Value,
                    Name = "E_Name",
                    Description = "E_Desc",
                    InitDateTime = DateTime.Today,
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                var result = await _modifyElectionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                var isSuccess = result is ModifyElectionSuccess;
                isSuccess.Should().BeTrue();
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfNameIsEmpty()
            {
                //Arrange
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = electionToModify.Id.Value,
                    Description = "E_Desc",
                    InitDateTime = DateTime.Today,
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                var result = await _modifyElectionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.NameIsEmpty.ToString());
                }
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfDescriptionIsEmpty()
            {
                //Arrange
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = electionToModify.Id.Value,
                    Name = "E_Name",
                    InitDateTime = DateTime.Today,
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                var result = await _modifyElectionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyElectionFailure failure)
                { 
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.Description.ToString());
                }
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfInitDateTimeIsEmpty()
            {
                //Arrange
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = electionToModify.Id.Value,
                    Name = "E_Name",
                    Description = "E_Desc",
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                var result = await _modifyElectionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.InitDateIsEmpty.ToString());
                }
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfFinalizeDateTimeIsEmpty()
            {
                //Arrange
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = electionToModify.Id.Value,
                    Name = "E_Name",
                    Description = "E_Desc",
                    InitDateTime = DateTime.Today,
                };

                //Act
                var result = await _modifyElectionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.FinishDateIsEmpty.ToString());
                }
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfIdIsEmpty()
            {
                //Arrange
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Name = "E_Name",
                    Description = "E_Desc",
                    InitDateTime = DateTime.Today,
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                var result = await _modifyElectionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.IdIsEmpty.ToString());
                }
            }


        }
    }
}
