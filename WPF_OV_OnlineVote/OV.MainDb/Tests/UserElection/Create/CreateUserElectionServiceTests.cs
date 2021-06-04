using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.UserElection;
using OV.MainDb.UserElection.Create;
using OV.MainDb.UserElection.Create.Models.Public;
using OV.MainDb.UserElection.Models.Public;
using System;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.UserElection.Create
{
    public class CreateUserElectionServiceTests
    {
        public class TestForCreateUserElectionService
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly ICreateUserElectionService _createUserElectionService;
            private readonly ICreateUserElectionDataService _createUserElectionDataService;
            private readonly ICandidateUserElectionValidator _validator;

            private readonly CancellationToken cancellationToken = default;

            public TestForCreateUserElectionService()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();


                _validator = new CandidateUserElectionValidator();
                _createUserElectionDataService = new CreateUserElectionDataService(_inMemoryOvMainDbContextFactory);

                _createUserElectionService = new CreateUserElectionService(_createUserElectionDataService, _validator);
            }

            [Fact]
            public async void ShouldCreateUserElection()
            {
                //Arrange
                CandidateUserElection candidate = new CandidateUserElection()
                {
                    TblUser_UID = 1,
                    TblElection_UID = 1
                };

                //Act
                ICreateUserElectionResponse result = await _createUserElectionService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isSuccess = result is CreateUserElectionSuccess;
                isSuccess.Should().BeTrue();
            }

            [Fact]
            public async void ShouldNotCreateUserElectionIfTblUser_UIDIsEmpty()
            {
                //Arrange
                CandidateUserElection candidate = new CandidateUserElection()
                {
                    TblElection_UID = 1
                };

                //Act
                ICreateUserElectionResponse result = await _createUserElectionService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateUserElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserElectionFailureReason.tblUser_UIDIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldNotCreateUserElectionIfTblelection_UIDIsEmpty()
            {
                //Arrange
                CandidateUserElection candidate = new CandidateUserElection()
                {
                    TblUser_UID = 1,
                };

                //Act
                ICreateUserElectionResponse result = await _createUserElectionService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateUserElectionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserElectionFailureReason.tblElection_UIDIsEmpty.ToString());
                }
            }
        }
    }
}
