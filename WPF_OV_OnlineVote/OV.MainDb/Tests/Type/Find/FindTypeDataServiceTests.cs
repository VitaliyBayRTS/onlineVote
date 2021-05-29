using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Type.Find;
using OV.MainDb.Type.Find.Models.Public;
using OV.MainDb.Type.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Type.Find
{
    public class FindTypeDataServiceTests
    {
        static PersistedType _dummyPersistedType_1 = new PersistedType()
        {
            Id = 1,
            Name = "name1",
            Description = "Desc1",
            Code = "V1"
        };
        static PersistedType _dummyPersistedType_2 = new PersistedType()
        {
            Id = 2,
            Name = "name2",
            Description = "Desc2",
            Code = "V2"
        };
        static PersistedType _dummyPersistedType_3 = new PersistedType()
        {
            Id = 3,
            Name = "name3",
            Description = "Desc3",
            Code = "V3"
        };


        public class TestForFindType
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindTypeDataService _findTypeDataService;
            private readonly CancellationToken cancellationToken = default;
            private List<PersistedType> arrayOfTypes = new List<PersistedType>();

            public TestForFindType()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findTypeDataService = new FindTypeDataService(_inMemoryOvMainDbContextFactory);

                arrayOfTypes.Add(_dummyPersistedType_1);
                arrayOfTypes.Add(_dummyPersistedType_2);
                arrayOfTypes.Add(_dummyPersistedType_3);
                _inMemoryOvMainDbContext.Types.AddRange(arrayOfTypes);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
            }


            [Fact]
            public async void ShoudlFindAllTypes()
            {
                //Arrange

                //Act
                var result = await _findTypeDataService.FindAsync(TypeFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfTypes.Count);
            }

            [Fact]
            public async void ShoudlFindByCode()
            {
                //Arrange
                var typeToFind = _dummyPersistedType_2;

                //Act
                var result = await _findTypeDataService.FindAsync(TypeFilter.ByCode(typeToFind.Code), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(typeToFind.Id);
                result.FirstOrDefault().Name.Should().Be(typeToFind.Name);
                result.FirstOrDefault().Description.Should().Be(typeToFind.Description);
            }
        }
    }
}
