using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Organizer;
using OV.MainDb.Organizer.Create;
using OV.MainDb.Organizer.Create.Models.Public;
using OV.MainDb.Organizer.Models.Public;
using System;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Organizer.Create
{
    public class CreateOrganizerServiceTest
    {
        public class TestForCreateOrganizerService
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly ICreateOrganizerService _createOrganizerService;
            private readonly ICreateOrganizerDataService _createOrganizerDataService;
            private readonly ICandidateOrganizerValidator _validator;

            private readonly CancellationToken cancellationToken = default;

            public TestForCreateOrganizerService()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();

                _validator = new CandidateOrganizerValidator();
                _createOrganizerDataService = new CreateOrganizerDataService(_inMemoryOvMainDbContextFactory);

                _createOrganizerService = new CreateOrganizerService(_createOrganizerDataService, _validator);
            }

            [Fact]
            public async void ShouldReturnSuccessResponse()
            {
                //Arrange
                CandidateOrganizer candidate = new CandidateOrganizer()
                {
                    tblUser_UID = 1,
                    tblElection_UID = 1,
                    ReferenceNumber = "RefNumber"
                };

                //Act
                var result = await _createOrganizerService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isSuccess = result is CreateOrganizerSuccess;
                isSuccess.Should().BeTrue();
            }
            [Fact]
            public async void ShouldFailureResponseIfUserIdIsEmpty()
            {
                //Arrange
                CandidateOrganizer candidate = new CandidateOrganizer()
                {
                    tblElection_UID = 1,
                    ReferenceNumber = "RefNumber"
                };

                //Act
                var result = await _createOrganizerService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateOrganizerFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OrganizerFailureReason.tblUser_UIDIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldFailureResponseIfElectionIdIsEmpty()
            {
                //Arrange
                CandidateOrganizer candidate = new CandidateOrganizer()
                {
                    tblUser_UID = 1,
                    ReferenceNumber = "RefNumber"
                };

                //Act
                var result = await _createOrganizerService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateOrganizerFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OrganizerFailureReason.tblElection_UIDIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldFailureResponseIfReferenceNumberIsEmpty()
            {
                //Arrange
                CandidateOrganizer candidate = new CandidateOrganizer()
                {
                    tblUser_UID = 1,
                    tblElection_UID = 1
                };

                //Act
                var result = await _createOrganizerService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateOrganizerFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OrganizerFailureReason.ReferenceNumberIsEmpty.ToString());
                }
            }

        }
    }
}
