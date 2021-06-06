using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election.GetUnnotified;
using OV.MainDb.Election.Models;
using OV.MainDb.Tests.Election.Find;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.GetUnnotified
{
    public class GetUnnotifiedDataServiceTests
    {
        public class TestForGetUnnotifiedElections
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IGetUnnotifiedDataService _getUnnotifiedDataService;
            private readonly CancellationToken cancellationToken = default;

            private List<PersistedElection> _arrayOfElections = new List<PersistedElection>();
            public TestForGetUnnotifiedElections()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _getUnnotifiedDataService = new GetUnnotifiedDataService(_inMemoryOvMainDbContextFactory);

                _arrayOfElections.Add(FindElectionPersistedGenerator._dummyPersistedElection_Unnotified);
                _arrayOfElections.Add(FindElectionPersistedGenerator._dummyPersistedElection_Notified);
                _inMemoryOvMainDbContext.Elections.AddRange(_arrayOfElections);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

            }


            [Fact]
            public async void ShoudlReturnAllUnnotified()
            {
                //Arrange
                List<PersistedElection> unnotifiedElections = _arrayOfElections.Where(e => (e.IsNotified == null && e.FinalizeDate < DateTime.Today) 
                    || (e.IsNotified.Value && e.FinalizeDate < DateTime.Today)).ToList();

                //Act
                var result = await _getUnnotifiedDataService.GetUnnotifiedAsync(cancellationToken);

                //Assert
                result.Should().HaveCount(unnotifiedElections.Count);
            }
        }
    }
}
