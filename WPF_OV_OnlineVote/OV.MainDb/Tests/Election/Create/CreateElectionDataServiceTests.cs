using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Create;
using OV.MainDb.Election.Models.Public;
using System;
 using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.Create
{
    public class CreateElectionDataServiceTests
    {
        public class TestForCreateElection 
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly ICreateElectionDataService _createElectionDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForCreateElection()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _createElectionDataService = new CreateElectionDataService(_inMemoryOvMainDbContextFactory);
            }

            [Fact]
            public async void ShouldCreateElection()
            {
                //Arrange
                CandidateElection candidate = new CandidateElection()
                {
                    Name = "E_Name",
                    Description = "E_Desc",
                    InitDate = DateTime.Today,
                    FinalizeDate = DateTime.Today.AddDays(1),
                    tblType_UID = 1
                };

                //Act
                var result = await _createElectionDataService.CreateAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(1);
                result.Name.Should().Be(candidate.Name);
                result.Description.Should().Be(candidate.Description);
                result.InitDate.Should().Be(candidate.InitDate);
                result.FinalizeDate.Should().Be(candidate.FinalizeDate);
                result.tblType_UID.Should().Be(candidate.tblType_UID);
            }

        }
    }
}
