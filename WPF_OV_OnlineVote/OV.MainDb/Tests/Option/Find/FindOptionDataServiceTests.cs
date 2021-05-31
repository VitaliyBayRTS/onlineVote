using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Find;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Option.Models;
using OV.MainDb.Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Option.Find
{
    public class FindOptionDataServiceTests
    {
        static PersistedOption _dummyOption1 = new PersistedOption()
        {
            Id = 1,
            tblElection_UID = 1,
            Name = "Option1",
            Description = "Desc1",
            Votes = default(int)
        };
        static PersistedOption _dummyOption2 = new PersistedOption()
        {
            Id = 2,
            tblElection_UID = 1,
            Name = "Option2",
            Description = "Desc2",
            Votes = default(int)
        };
        static PersistedOption _dummyOption3 = new PersistedOption()
        {
            Id = 3,
            tblElection_UID = 3,
            Name = "Option3",
            Description = "Desc3",
            Votes = default(int)
        };
        public class TestForFindOption
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindOptionDataService _findOptionDataService;
            private readonly CancellationToken cancellationToken = default;
            private List<PersistedOption> arrayOfOptions = new List<PersistedOption>();

            public TestForFindOption()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findOptionDataService = new FindOptionDataService(_inMemoryOvMainDbContextFactory);


                arrayOfOptions.Add(_dummyOption1);
                arrayOfOptions.Add(_dummyOption2);
                arrayOfOptions.Add(_dummyOption3);
                _inMemoryOvMainDbContext.Options.AddRange(arrayOfOptions);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
            }

            [Fact]
            public async void ShoudlFindAllOptions()
            {
                //Arrange
                //Act
                var result = await _findOptionDataService.FindAsync(OptionFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfOptions.Count);
            }

            [Fact]
            public async void ShoudlFindOptionById()
            {
                //Arrange
                var optionToFind = _dummyOption2;
                var IdToFind = optionToFind.Id;

                //Act
                var result = await _findOptionDataService.FindAsync(OptionFilter.ById(IdToFind.Value), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(optionToFind.Id);
                result.FirstOrDefault().tblElection_UID.Should().Be(optionToFind.tblElection_UID);
            }
        }
    }
}
