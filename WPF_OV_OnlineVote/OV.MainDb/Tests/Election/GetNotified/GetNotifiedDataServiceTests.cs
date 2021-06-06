using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election.GetNotified;
using OV.MainDb.Election.Models;
using OV.MainDb.Tests.Election.Find;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.GetNotified
{
    public class GetNotifiedDataServiceTests
    {
        public class TestsForGetnotifiedElections
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IGetNotifiedDataService _getNotifiedDataService;
            private readonly CancellationToken cancellationToken = default;

            private List<PersistedElection> _arrayOfElections = new List<PersistedElection>();
            public TestsForGetnotifiedElections()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _getNotifiedDataService = new GetNotifiedDataService(_inMemoryOvMainDbContextFactory);

                _arrayOfElections.Add(FindElectionPersistedGenerator._dummyPersistedElection_Unnotified);
                _arrayOfElections.Add(FindElectionPersistedGenerator._dummyPersistedElection_Notified);
                _inMemoryOvMainDbContext.Elections.AddRange(_arrayOfElections);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

            }


            [Fact]
            public async void ShoudlReturnAllNotified()
            {
                //Arrange
                List<PersistedElection> notifiedElections = _arrayOfElections.Where(e => e.IsNotified != null && e.IsNotified.Value).ToList();

                //Act
                var result = await _getNotifiedDataService.GetNotifiedAsync(cancellationToken);

                //Assert
                result.Should().HaveCount(notifiedElections.Count);
            }
        }
    }
}
