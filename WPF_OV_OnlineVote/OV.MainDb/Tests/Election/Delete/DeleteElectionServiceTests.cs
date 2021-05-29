using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Delete;
using OV.MainDb.Election.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.Delete
{
    public class DeleteElectionServiceTests
    {
        private static PersistedElection _dummyPersistedElection1 = new PersistedElection()
        {
            Id = 1,
            Name = "E_Name1",
            Description = "E_Desc1",
            InitDate = DateTime.Today,
            FinalizeDate = DateTime.Today.AddDays(1),
            tblType_UID = 1
        };
        private static PersistedElection _dummyPersistedElection2 = new PersistedElection()
        {
            Id = 2,
            Name = "E_Name1",
            Description = "E_Desc1",
            InitDate = DateTime.Today,
            FinalizeDate = DateTime.Today.AddDays(1),
            tblType_UID = 1
        };

        public class TestForDeleteElection
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _ovMainDbContext;
            private readonly IDeleteElectionService _deleteElectionService;
            private readonly CancellationToken cancellationToken = default;

            public TestForDeleteElection()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _ovMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _deleteElectionService = new DeleteElectionService(new DeleteElectionDataService(_inMemoryOvMainDbContextFactory));
            }


            [Fact]
            public async void ShouldDeleteElection()
            {
                //Arrange
                List<PersistedElection> elections = new List<PersistedElection>();
                elections.Add(_dummyPersistedElection1);
                elections.Add(_dummyPersistedElection2);
                _ovMainDbContext.Elections.AddRange(elections);
                await _ovMainDbContext.SaveChangesAsync();

                var electionIdToRemove = _dummyPersistedElection1.Id;

                //Act
                var result = await _deleteElectionService.DeleteAsync(electionIdToRemove.Value, cancellationToken);

                //Assert
                result.Should().BeTrue();
                _ovMainDbContext.Elections.Where(e => true).ToList().Should().HaveCount(elections.Count - 1);
            }

            [Theory]
            [InlineData(123)]
            [InlineData(default(int))]
            [InlineData(-123)]
            public async void ShouldReturnFalseIfElectionIdIsWrong(int tblElection_UID)
            {
                //Arrange

                //Act
                var result = await _deleteElectionService.DeleteAsync(tblElection_UID, cancellationToken);

                //Assert
                result.Should().BeFalse();
            }
        }
    }
}
