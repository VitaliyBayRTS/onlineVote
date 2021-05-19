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
        static PersistedUser _dummyUser1 = new PersistedUser()
        {
            Id = 100,
            DNI_NIE = "12345678z",
            DOB = DateTime.Now,
            Email = "asd@asd",
            FirstName = "FirstName1",
            IsAutorized = false,
            Password = "asdasdasd",
            PhoneNumber = "123987685",
            SurName = "SurName",
            TblProvince_UID = 100
        };
        static PersistedUser _dummyUser2 = new PersistedUser()
        {
            Id = 200,
            DNI_NIE = "12345678z",
            DOB = DateTime.Now,
            Email = "asd@asd",
            FirstName = "FirstName2",
            IsAutorized = false,
            Password = "asdasdasd",
            PhoneNumber = "123987685",
            SurName = "SurName",
            TblProvince_UID = 100
        };
        static PersistedUser _dummyUser3 = new PersistedUser()
        {
            Id = 300,
            DNI_NIE = "12345678z",
            DOB = DateTime.Now,
            Email = "asd@asd",
            FirstName = "FirstName3",
            IsAutorized = true,
            Password = "asdasdasd",
            PhoneNumber = "123987685",
            SurName = "SurName",
            TblProvince_UID = 100
        };

        public class TestForFindHabitant
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindUserDataService _findUserDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindHabitant()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findUserDataService = new FindUserDataService(_inMemoryOvMainDbContextFactory);
            }

            [Fact]
            public async void ShoudlFindAllUsers()
            {
                //Arrange
                var arrayOfUsers = new List<PersistedUser>();
                arrayOfUsers.Add(_dummyUser1);
                arrayOfUsers.Add(_dummyUser2);
                arrayOfUsers.Add(_dummyUser3);
                _inMemoryOvMainDbContext.Users.AddRange(arrayOfUsers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfUsers.Count);
            }

            [Fact]
            public async void ShoudlFindUserById()
            {
                //Arrange
                var arrayOfUsers = new List<PersistedUser>();
                arrayOfUsers.Add(_dummyUser1);
                arrayOfUsers.Add(_dummyUser2);
                arrayOfUsers.Add(_dummyUser3);
                _inMemoryOvMainDbContext.Users.AddRange(arrayOfUsers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var userToFind = _dummyUser1;
                var IdToFind = userToFind.Id;

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.ById(IdToFind.Value), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(IdToFind);
                result.FirstOrDefault().DNI_NIE.Should().Be(userToFind.DNI_NIE);
            }

            [Fact]
            public async void ShoudlFindUnautorizedUsers()
            {
                //Arrange
                var arrayOfUsers = new List<PersistedUser>();
                arrayOfUsers.Add(_dummyUser1);
                arrayOfUsers.Add(_dummyUser2);
                arrayOfUsers.Add(_dummyUser3);
                _inMemoryOvMainDbContext.Users.AddRange(arrayOfUsers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var unautorizedUsersCount = arrayOfUsers.Where(u => !u.IsAutorized).ToList().Count;

                //Act
                var result = await _findUserDataService.FindAsync(UserFilter.ByUnautorized(), cancellationToken);

                //Assert
                result.Should().HaveCount(unautorizedUsersCount);
                result.All(u => !u.IsAutorized).Should().BeTrue();
            }

        }
    }
}
