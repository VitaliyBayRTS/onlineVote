using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.User.Find;
using OV.MainDb.User.Find.Models.Public;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.User.Find
{
    public class FindUserDataServiceTests
    {

        public class TestForFindHabitant
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindUserDataService _findUserDataService;
            private readonly CancellationToken cancellationToken = default;

            private List<PersistedUser> _arrayOfUsers = new List<PersistedUser>();

            public TestForFindHabitant()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findUserDataService = new FindUserDataService(_inMemoryOvMainDbContextFactory);


                _inMemoryOvMainDbContext.Provinces.AddRange(FindUserPersistedGenerator._dummyProvince);
                _arrayOfUsers.Add(FindUserPersistedGenerator._dummyUser1);
                _arrayOfUsers.Add(FindUserPersistedGenerator._dummyUser2);
                _arrayOfUsers.Add(FindUserPersistedGenerator._dummyUser3);
                _inMemoryOvMainDbContext.Users.AddRange(_arrayOfUsers);
                _inMemoryOvMainDbContext.SaveChangesAsync();
            }

            [Fact]
            public async void ShoudlFindAllUsers()
            {
                //Arrange

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfUsers.Count);
                result.All(u => u.Province == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindUserById()
            {
                //Arrange
                var userToFind = FindUserPersistedGenerator._dummyUser1;
                var IdToFind = userToFind.Id;

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.ById(IdToFind.Value), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(IdToFind);
                result.FirstOrDefault().DNI_NIE.Should().Be(userToFind.DNI_NIE);
                result.All(u => u.Province == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindUnautorizedUsers()
            {
                //Arrange
                var unautorizedUsersCount = _arrayOfUsers.Where(u => !u.IsAutorized).ToList().Count;

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.ByUnautorized(), cancellationToken);

                //Assert
                result.Should().HaveCount(unautorizedUsersCount);
                result.All(u => !u.IsAutorized).Should().BeTrue();
                result.All(u => u.Province == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAutorizedUsers()
            {
                //Arrange
                var autorizedUsersCount = _arrayOfUsers.Where(u => u.IsAutorized).ToList().Count;

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.ByAutorized(), cancellationToken);

                //Assert
                result.Should().HaveCount(autorizedUsersCount);
                result.All(u => u.IsAutorized).Should().BeTrue();
                result.All(u => u.Province == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAllUsersAndIncludeProvince()
            {
                //Arrange

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.All.AndByIncludeProvince(), cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfUsers.Count);
                result.All(u => u.Province != null).Should().BeTrue();
                result.All(u => u.Province.AutonomousCommunity == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindAllUsersAndIncludeAC()
            {
                //Arrange

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.All.AndByIncludeProvince().AndByIncludeAC(), cancellationToken);

                //Assert
                result.Should().HaveCount(_arrayOfUsers.Count);
                result.All(u => u.Province != null).Should().BeTrue();
                result.All(u => u.Province.AutonomousCommunity != null).Should().BeTrue();
            }

        }
    }
}
