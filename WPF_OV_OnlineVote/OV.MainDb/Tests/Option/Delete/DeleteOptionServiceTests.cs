using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Delete;
using OV.MainDb.Option.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Option.Delete
{
    public class DeleteOptionServiceTests
    {
        static PersistedOption _dummyOption1 = new PersistedOption()
        {
            Id = 1,
            tblElection_UID = 1,
            Name = "Name1",
            Description = "Desc1",
            Votes = default(int)
        };
        static PersistedOption _dummyOption2 = new PersistedOption()
        {
            Id = 2,
            tblElection_UID = 1,
            Name = "Name1",
            Description = "Desc1",
            Votes = default(int)
        };
        static PersistedOption _dummyOption3 = new PersistedOption()
        {
            Id = 3,
            tblElection_UID = 1,
            Name = "Name1",
            Description = "Desc1",
            Votes = default(int)
        };
        static PersistedOption _dummyOption4 = new PersistedOption()
        {
            Id = 4,
            tblElection_UID = 1,
            Name = "Name1",
            Description = "Desc1",
            Votes = default(int)
        };


        public class TestForDeleteOption
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _ovMainDbContext;
            private readonly IDeleteOptionService _deleteOptionService;
            private readonly CancellationToken cancellationToken = default;

            public TestForDeleteOption()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _ovMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _deleteOptionService = new DeleteOptionService(new DeleteOptionDataService(_inMemoryOvMainDbContextFactory));
            }


            [Fact]
            public async void ShouldDeleteUser()
            {
                //Arrange
                List<PersistedOption> options = new List<PersistedOption>();
                options.Add(_dummyOption1);
                options.Add(_dummyOption2);
                options.Add(_dummyOption3);
                options.Add(_dummyOption4);
                _ovMainDbContext.Options.AddRange(options);
                await _ovMainDbContext.SaveChangesAsync();

                var userIdToRemove = _dummyOption2.Id;

                //Act
                var result = await _deleteOptionService.DeleteAsync(userIdToRemove.Value, cancellationToken);

                //Assert
                result.Should().BeTrue();
                _ovMainDbContext.Options.Where(e => true).ToList().Should().HaveCount(options.Count - 1);
            }


            [Theory]
            [InlineData(656)]
            [InlineData(-433)]
            [InlineData(default(int))]
            public async void ShouldReturnFalseIfuserIdIsWrong(int tblUser_UID)
            {
                //Arrange

                //Act
                var result = await _deleteOptionService.DeleteAsync(tblUser_UID, cancellationToken);

                //Assert
                result.Should().BeFalse();
            }
        }
    }
}
