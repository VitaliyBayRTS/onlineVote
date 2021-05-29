using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Tests.User.Find;
using OV.MainDb.User.Delete;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.User.Delete
{
    public class DeleteUserServiceTests
    {
        public class TestForDeleteUser
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _ovMainDbContext;
            private readonly IDeleteUserService _deleteUserService;
            private readonly CancellationToken cancellationToken = default;

            public TestForDeleteUser()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _ovMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _deleteUserService = new DeleteUserService(new DeleteUserDataService(_inMemoryOvMainDbContextFactory));
            }


            [Fact]
            public async void ShouldDeleteUser()
            {
                //Arrange
                List<PersistedUser> users = new List<PersistedUser>();
                users.Add(FindUserPersistedGenerator._dummyUser1);
                users.Add(FindUserPersistedGenerator._dummyUser2);
                users.Add(FindUserPersistedGenerator._dummyUser3);
                _ovMainDbContext.Users.AddRange(users);
                await _ovMainDbContext.SaveChangesAsync();

                var userIdToRemove = FindUserPersistedGenerator._dummyUser2.Id;

                //Act
                var result = await _deleteUserService.DeleteAsync(userIdToRemove.Value, cancellationToken);

                //Assert
                result.Should().BeTrue();
                _ovMainDbContext.Users.Where(e => true).ToList().Should().HaveCount(users.Count - 1);
            }


            [Theory]
            [InlineData(656)]
            [InlineData(-433)]
            [InlineData(default(int))]
            public async void ShouldReturnFalseIfuserIdIsWrong(int tblUser_UID)
            {
                //Arrange

                //Act
                var result = await _deleteUserService.DeleteAsync(tblUser_UID, cancellationToken);

                //Assert
                result.Should().BeFalse();
            }
        }
    }
}
