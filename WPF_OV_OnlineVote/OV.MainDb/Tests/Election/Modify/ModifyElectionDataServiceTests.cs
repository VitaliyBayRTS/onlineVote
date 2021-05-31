using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Modify;
using OV.MainDb.Election.Modify.Models.Public;
using OV.MainDb.Tests.Election.Find;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OV.MainDb.Tests.Election.Modify
{
    public class ModifyElectionDataServiceTests
    {
        public class TestForModifyElection
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IModifyElectionDataService _modifyElectionDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForModifyElection()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _modifyElectionDataService = new ModifyElectionDataService(_inMemoryOvMainDbContextFactory);
            }

            [Fact]
            public async void ShouldModifyElection()
            {
                //Arrange
                var electionToModify = FindElectionPersistedGenerator._dummyPersistedElection1;
                _inMemoryOvMainDbContext.Elections.Add(electionToModify);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = electionToModify.Id.Value,
                    Name = "E_Name",
                    Description = "E_Desc",
                    InitDateTime = DateTime.Today,
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                var result = await _modifyElectionDataService.ModifyAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(candidate.Id);
                result.Name.Should().Be(candidate.Name);
                result.Description.Should().Be(candidate.Description);
                result.InitDate.Should().Be(candidate.InitDateTime);
                result.FinalizeDate.Should().Be(candidate.FinalizeDateTime);
            }

            [Fact]
            public async void ShouldNotModifyElectionIfIdDoesNotExist()
            {
                //Arrange
                var electionToModify = FindElectionPersistedGenerator._dummyPersistedElection1;
                _inMemoryOvMainDbContext.Elections.Add(electionToModify);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = 9999,
                    Name = "E_Name",
                    Description = "E_Desc",
                    InitDateTime = DateTime.Today,
                    FinalizeDateTime = DateTime.Today.AddDays(1),
                };

                //Act
                Func<Task> act = async () => await _modifyElectionDataService.ModifyAsync(candidate, cancellationToken);

                //Assert
                await act.Should().ThrowAsync<InvalidOperationException>();

            }
        }
    }
}
