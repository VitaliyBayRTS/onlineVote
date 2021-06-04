using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.UserElection.Create;
using OV.MainDb.UserElection.Models.Public;
using System;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.UserElection.Create
{
    public class CreateUserElectionDataServiceTest
    {
        public class TestForCreateUserElection
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly ICreateUserElectionDataService _createUserElectionDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForCreateUserElection()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _createUserElectionDataService = new CreateUserElectionDataService(_inMemoryOvMainDbContextFactory);
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
                var result = await _createUserElectionDataService.CreateAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(1);
                result.TblUser_UID.Should().Be(candidate.TblUser_UID);
                result.TblElection_UID.Should().Be(candidate.TblElection_UID);
            }
        }
    }
}
