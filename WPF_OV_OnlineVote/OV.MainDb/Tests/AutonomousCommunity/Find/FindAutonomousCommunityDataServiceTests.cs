using FluentAssertions;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.AutonomousCommunity.Find
{
    public class FindAutonomousCommunityDataServiceTests
    {
        static PersistedAutonomousCommunity _dummyAutonomousCommunity1 = new PersistedAutonomousCommunity()
        {
            Id = 1,
            Name = "Name1",
            Provinces = new List<PersistedProvince>()
            {
                new PersistedProvince()
                {
                    Name = "ProvinceName1"
                },
                new PersistedProvince()
                {
                    Name = "ProvinceName2"
                },
                new PersistedProvince()
                {
                    Name = "ProvinceName3"
                }
            }
        };
        static PersistedAutonomousCommunity _dummyAutonomousCommunity2 = new PersistedAutonomousCommunity()
        {
            Id = 2,
            Name = "Name2",
            Provinces = new List<PersistedProvince>()
            {
                new PersistedProvince()
                {
                    Name = "ProvinceName2_1"
                }
            }
        };
        static PersistedAutonomousCommunity _dummyAutonomousCommunity3 = new PersistedAutonomousCommunity()
        {
            Id = 3,
            Name = "Name3",
            Provinces = new List<PersistedProvince>()
            {
                new PersistedProvince()
                {
                    Name = "ProvinceName3_1"
                },
                new PersistedProvince()
                {
                    Name = "ProvinceName3_2"
                }
            }
        };
        static PersistedAutonomousCommunity _dummyAutonomousCommunity4 = new PersistedAutonomousCommunity()
        {
            Id = 4,
            Name = "Name4"
        };

        public class TestForFindAutonomousCommunity
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindAutonomousCommunityDataService _findAutonomousCommunityDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindAutonomousCommunity()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findAutonomousCommunityDataService = new FindAutonomousCommunityDataService(_inMemoryOvMainDbContext);

            }

            [Fact]
            public async void ShoudlReturnAllObjects()
            {
                //Arrange
                List<PersistedAutonomousCommunity> acs = new List<PersistedAutonomousCommunity>();
                acs.Add(_dummyAutonomousCommunity1);
                acs.Add(_dummyAutonomousCommunity2);
                acs.Add(_dummyAutonomousCommunity3);
                _inMemoryOvMainDbContext.AutonomousCommunities.AddRange(acs);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);



                //Act
                var result = await _findAutonomousCommunityDataService.FindAsync(AutonomousCommunityFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(acs.Count);
                result.All(ac => ac.Provinces == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAutonomousCommunityByIdIfObjectWithThisIdExists()
            {
                //Arrange
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity1);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity2);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity3);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var autonomousCommunityId = _dummyAutonomousCommunity2.Id;


                //Act
                var result = await _findAutonomousCommunityDataService.FindAsync(AutonomousCommunityFilter.ById(autonomousCommunityId), cancellationToken);

                //Assert
                result.FirstOrDefault().Id.Should().Be(autonomousCommunityId);
                result.All(ac => ac.Provinces == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAutonomousCommunityByNameIfObjectWithThisNameExists()
            {
                //Arrange
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity1);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity2);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity3);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var autonomousCommunityName = _dummyAutonomousCommunity3.Name;


                //Act
                var result = await _findAutonomousCommunityDataService.FindAsync(AutonomousCommunityFilter.ByName(autonomousCommunityName), cancellationToken);

                //Assert
                result.FirstOrDefault().Name.Should().Be(autonomousCommunityName);
                result.All(ac => ac.Provinces == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlIncludeProvinces()
            {
                //Arrange
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity1);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity2);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity3);
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(_dummyAutonomousCommunity4);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                //Act
                var result = await _findAutonomousCommunityDataService.FindAsync(AutonomousCommunityFilter.All.AndIncludeProvince(), cancellationToken);

                //Assert
                result.All(ac => ac.Provinces != null).Should().BeTrue();
            }

        }
    }
}
