using FluentAssertions;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Find;
using OV.MainDb.Province.Find.Models.Public;
using OV.MainDb.Province.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.AutonomousCommunity.Find
{
    public class FindProvinceDataServiceTests
    {
        static PersistedProvince _dummyProvince1 = new PersistedProvince()
        {
            Id = 1,
            Name = "Name1",
            AutonomousCommunity = new PersistedAutonomousCommunity()
            {
                Name = "ACName1"
            }
        };
        static PersistedProvince _dummyProvince2 = new PersistedProvince()
        {
            Id = 2,
            Name = "Name2",
            AutonomousCommunity = new PersistedAutonomousCommunity()
            {
                Name = "ACName2_1"
            }
        };
        static PersistedProvince _dummyProvince3 = new PersistedProvince()
        {
            Id = 3,
            Name = "Name3",
            AutonomousCommunity = new PersistedAutonomousCommunity()
            {
                Name = "ACName3_1"
            }
        };
        static PersistedProvince _dummyProvince4 = new PersistedProvince()
        {
            Id = 4,
            Name = "Name4"
        };

        public class TestForFindProvinces
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindProvinceDataService _findProvinceDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindProvinces()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findProvinceDataService = new FindProvinceDataService(_inMemoryOvMainDbContext);

            }

            [Fact]
            public async void ShoudlFindProvinceByIdIfObjectWithThisIdExists()
            {
                //Arrange
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince1);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince2);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince3);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var provinceId = _dummyProvince2.Id;


                //Act
                var result = await _findProvinceDataService.FindAsync(ProvinceFilter.ById(provinceId), cancellationToken);

                //Assert
                result.FirstOrDefault().Id.Should().Be(provinceId);
                result.All(p => p.AutonomousCommunity == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindProvinceByNameIfObjectWithThisNameExists()
            {
                //Arrange
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince1);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince2);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince3);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var ProvinceName = _dummyProvince3.Name;


                //Act
                var result = await _findProvinceDataService.FindAsync(ProvinceFilter.ByName(ProvinceName), cancellationToken);

                //Assert
                result.FirstOrDefault().Name.Should().Be(ProvinceName);
                result.All(p => p.AutonomousCommunity == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlIncludeAutonomousCommunity()
            {
                //Arrange
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince1);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince2);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince3);
                _inMemoryOvMainDbContext.Provinces.Add(_dummyProvince4);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                //Act
                var result = await _findProvinceDataService.FindAsync(ProvinceFilter.All.AndIncludeAutonomousCommunity(), cancellationToken);

                //Assert
                result.All(p => p.AutonomousCommunity != null).Should().BeTrue();
            }

        }
    }
}
