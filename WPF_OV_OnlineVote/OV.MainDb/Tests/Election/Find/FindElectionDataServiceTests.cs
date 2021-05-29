using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Find;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Election.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Election.Find
{
    public class FindElectionDataServiceTests
    {
        public class TestForFindElection
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindElectionDataService _findElectionDataService;
            private readonly CancellationToken cancellationToken = default;

            private List<PersistedElection> _arrayOfElections = new List<PersistedElection>();
            public TestForFindElection()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findElectionDataService = new FindElectionDataService(_inMemoryOvMainDbContextFactory);

                _arrayOfElections.Add(FindElectionPersistedGenerator._dummyPersistedElection1);
                _arrayOfElections.Add(FindElectionPersistedGenerator._dummyPersistedElection2);
                _inMemoryOvMainDbContext.Elections.AddRange(_arrayOfElections);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

            }


            [Fact]
            public async void ShoudlFindAllElections()
            {
                //Arrange

                //Act
                var result = await _findElectionDataService.FindAsync(ElectionFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfElections.Count);
                result.All(e => e.Type == null).Should().BeTrue();
                result.All(e => e.Province == null).Should().BeTrue();
                result.All(e => e.AutonomousCommunity == null).Should().BeTrue();
                result.All(e => e.Organizers == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAllElectionsAndIncludeType()
            {
                //Arrange

                //Act
                var result = await _findElectionDataService.FindAsync(ElectionFilter.All.AndTypeIncluded(), cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfElections.Count);
                result.All(e => e.Type != null).Should().BeTrue();
                result.All(e => e.Province == null).Should().BeTrue();
                result.All(e => e.AutonomousCommunity == null).Should().BeTrue();
                result.All(e => e.Organizers == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAllElectionsAndIncludeProvince()
            {
                //Arrange

                //Act
                var result = await _findElectionDataService.FindAsync(ElectionFilter.All.AndProvinceIncluded(), cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfElections.Count);
                result.All(e => e.Type == null).Should().BeTrue();
                result.All(e => e.Province != null).Should().BeTrue();
                result.All(e => e.AutonomousCommunity == null).Should().BeTrue();
                result.All(e => e.Organizers == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAllElectionsAndIncludeAC()
            {
                //Arrange

                //Act
                var result = await _findElectionDataService.FindAsync(ElectionFilter.All.AndACIncluded(), cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfElections.Count);
                result.All(e => e.Type == null).Should().BeTrue();
                result.All(e => e.Province == null).Should().BeTrue();
                result.All(e => e.AutonomousCommunity != null).Should().BeTrue();
                result.All(e => e.Organizers == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAllElectionsAndIncludeOrganizers()
            {
                //Arrange

                //Act
                var result = await _findElectionDataService.FindAsync(ElectionFilter.All.AndOrganizersIncluded(), cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfElections.Count);
                result.All(e => e.Type == null).Should().BeTrue();
                result.All(e => e.Province == null).Should().BeTrue();
                result.All(e => e.AutonomousCommunity == null).Should().BeTrue();
                result.All(e => e.Organizers != null).Should().BeTrue();
            }
        }
    }
}
