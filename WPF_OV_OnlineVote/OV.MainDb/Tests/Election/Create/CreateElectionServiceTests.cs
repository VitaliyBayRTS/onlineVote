using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election;
using OV.MainDb.Election.Create;
using OV.MainDb.Election.Create.Models.Public;
using OV.MainDb.Election.Models.Public;
using OV.MainDb.Type.Models;
using System;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.Create
{
    public class CreateElectionServiceTests
    {
        public class TestForCreateOrganizerService
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly ICreateElectionDataService _createElectionDataService;
            private readonly ICandidateElectionValidator _candidateElectionValidator;
            private readonly ICreateElectionService _createElectionService;

            private readonly CancellationToken cancellationToken = default;

            public TestForCreateOrganizerService()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();


                _candidateElectionValidator = new CandidateElectionValidator();
                _createElectionDataService = new CreateElectionDataService(_inMemoryOvMainDbContextFactory);

                _createElectionService = new CreateElectionService(_createElectionDataService, _candidateElectionValidator);
            }


            [Fact]
            public async void ShouldCreateElection()
            {
                //Arrange
                var type = _inMemoryOvMainDbContext.Types.Add(new PersistedType()
                {
                    Name = "Name",
                    Code = "N",
                    Description = "Desc"
                });
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateElection candidateElection = new CandidateElection()
                {
                    Name = "TestName",
                    Description = "TestDesc",
                    InitDate = DateTime.Today,
                    FinalizeDate = DateTime.Today.AddDays(1),
                    tblType_UID = type.Entity.Id
                };

                //Act
                ICreateElectionResponse result = await _createElectionService.CreateAsync(candidateElection, cancellationToken);

                //Assert
                var isSuccess = result is CreateElectionSuccess;
                isSuccess.Should().BeTrue();
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfNameIsEmpty()
            {
                //Arrange
                var type = _inMemoryOvMainDbContext.Types.Add(new PersistedType()
                {
                    Name = "Name",
                    Code = "N",
                    Description = "Desc"
                });
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateElection candidateElection = new CandidateElection()
                {
                    Description = "TestDesc",
                    InitDate = DateTime.Today,
                    FinalizeDate = DateTime.Today.AddDays(1),
                    tblType_UID = type.Entity.Id
                };

                //Act
                ICreateElectionResponse result = await _createElectionService.CreateAsync(candidateElection, cancellationToken);

                //Assert
                if (result is CreateElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.NameIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfDescriptionIsEmpty()
            {
                //Arrange
                var type = _inMemoryOvMainDbContext.Types.Add(new PersistedType()
                {
                    Name = "Name",
                    Code = "N",
                    Description = "Desc"
                });
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateElection candidateElection = new CandidateElection()
                {
                    Name = "TestName",
                    InitDate = DateTime.Today,
                    FinalizeDate = DateTime.Today.AddDays(1),
                    tblType_UID = type.Entity.Id
                };

                //Act
                ICreateElectionResponse result = await _createElectionService.CreateAsync(candidateElection, cancellationToken);

                //Assert
                if (result is CreateElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.Description.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfInitDateIsEmpty()
            {
                //Arrange
                var type = _inMemoryOvMainDbContext.Types.Add(new PersistedType()
                {
                    Name = "Name",
                    Code = "N",
                    Description = "Desc"
                });
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateElection candidateElection = new CandidateElection()
                {
                    Name = "TestName",
                    Description = "TestDesc",
                    FinalizeDate = DateTime.Today.AddDays(1),
                    tblType_UID = type.Entity.Id
                };

                //Act
                ICreateElectionResponse result = await _createElectionService.CreateAsync(candidateElection, cancellationToken);

                //Assert
                if (result is CreateElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.InitDateIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfFinishDateIsEmpty()
            {
                //Arrange
                var type = _inMemoryOvMainDbContext.Types.Add(new PersistedType()
                {
                    Name = "Name",
                    Code = "N",
                    Description = "Desc"
                });
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateElection candidateElection = new CandidateElection()
                {
                    Name = "TestName",
                    Description = "TestDesc",
                    InitDate = DateTime.Today,
                    tblType_UID = type.Entity.Id
                };

                //Act
                ICreateElectionResponse result = await _createElectionService.CreateAsync(candidateElection, cancellationToken);

                //Assert
                if (result is CreateElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.FinishDateIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfTypeIsEmpty()
            {
                //Arrange
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateElection candidateElection = new CandidateElection()
                {
                    Name = "TestName",
                    Description = "TestDesc",
                    InitDate = DateTime.Today,
                    FinalizeDate = DateTime.Today.AddDays(1),
                };

                //Act
                ICreateElectionResponse result = await _createElectionService.CreateAsync(candidateElection, cancellationToken);

                //Assert
                if (result is CreateElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(ElectionFailureReason.TypeIsEmpty.ToString());
                }
            }



        }
    }
}
