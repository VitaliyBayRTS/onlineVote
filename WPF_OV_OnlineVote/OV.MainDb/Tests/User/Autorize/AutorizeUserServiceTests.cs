using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Tests.User.Find;
using OV.MainDb.User.Autorize;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.User.Autorize
{
    public class AutorizeUserServiceTests
    {
        public class testForAutorizeuser
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _ovMainDbContext;
            private readonly IAutorizeUserService _autorizeUserService;
            private readonly CancellationToken cancellationToken = default;

            public testForAutorizeuser()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _ovMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _autorizeUserService = new AutorizeUserService(new AutorizeUserDataService(_inMemoryOvMainDbContextFactory));
            }

            [Fact]
            public async void ShouldUpdateAutorizePropertyOfUser()
            {
                //Arrange
                List<PersistedUser> users = new List<PersistedUser>();
                users.Add(FindUserPersistedGenerator._dummyUser1);
                users.Add(FindUserPersistedGenerator._dummyUser2);
                users.Add(FindUserPersistedGenerator._dummyUser3);
                _ovMainDbContext.Users.AddRange(users);
                await _ovMainDbContext.SaveChangesAsync();

                var userThatShouldBeAutorized = users.First(u => u.IsAutorized);
                if (userThatShouldBeAutorized == null)
                    throw new NullReferenceException("You should have at least one user that is not autorized");
                var userId = userThatShouldBeAutorized.Id;

                //Act
                var result = await _autorizeUserService.AutorizeAsync(userId.Value, cancellationToken);

                //Assert
                result.Should().BeTrue();
                _ovMainDbContext.Users.First(u => u.Id == userThatShouldBeAutorized.Id).IsAutorized.Should().BeTrue();
            }


            [Theory]
            [InlineData(443)]
            [InlineData(-3)]
            [InlineData(default(int))]
            public async void ShouldReturnFalseIfUserIdIsWrong(int tblUser_UID)
            {
                //Arrange

                //Act
                var result = await _autorizeUserService.AutorizeAsync(tblUser_UID, cancellationToken);

                //Assert
                result.Should().BeFalse();
            }
        }
    }
}
