using FluentAssertions;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Habitant.Find;
using OV.MainDb.Habitant.Find.Models.Public;
using OV.MainDb.Habitant.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Habitant.Find
{
    public class FindHabitantDataServiceTests
    {
        static PersistedHabitant _dummyHabitant1 = new PersistedHabitant()
        {
            Id = 1,
            tblUser_UID = 100,
            User = new PersistedUser()
            {
                Id = 100,
                DNI_NIE = "12345678z",
                DOB = DateTime.Now,
                Email = "asd@asd",
                FirstName = "FirstName",
                IsAutorized = true,
                Password = "asdasdasd",
                PhoneNumber = "123987685",
                SurName = "SurName",
                TblProvince_UID = 100
            }
        };
        static PersistedHabitant _dummyHabitant2 = new PersistedHabitant()
        {
            Id = 2,
            tblUser_UID = 200,
            User = new PersistedUser()
            {
                Id = 200,
                DNI_NIE = "1111111z",
                DOB = DateTime.Now,
                Email = "asd@asd",
                FirstName = "FirstName2",
                IsAutorized = true,
                Password = "asdasdasd",
                PhoneNumber = "123987685",
                SurName = "SurName2",
                TblProvince_UID = 100,
            }
        };
        public class TestForFindHabitant
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindHabitantDataService _findHabitantDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindHabitant()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findHabitantDataService = new FindHabitantDataService(_inMemoryOvMainDbContextFactory);

            }

            [Fact]
            public async void ShoudlFindAllHabitants()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfHabitants.Count);
                result.All(h => h.User == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindHabitantById()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var habitantToFind = _dummyHabitant1;
                var IdToFind = habitantToFind.Id;

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.ById(IdToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(habitantToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByIdIfIdNoesNotExist()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var unexistingId = 1568;

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.ById(unexistingId), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }


            [Fact]
            public async void ShoudlFindHabitantByDNI_NIE()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var habitantToFind = _dummyHabitant1;
                var DNI_NIEToFind = habitantToFind.User.DNI_NIE;

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.ByDNI_NIE(DNI_NIEToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(habitantToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIEIfDNIE_NIE_DoesNotexist()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var DNI_NIEToFind = "58258369D";

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.ByDNI_NIE(DNI_NIEToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlIncludeUser()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);


                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.All.AndIncludeUser(), cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfHabitants.Count);
                result.All(h => h.User != null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindHabitantByDNI_NIEAndPassword()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var habitantToFind = _dummyHabitant1;
                var DNI_NIEToFind = habitantToFind.User.DNI_NIE;
                var Password = habitantToFind.User.Password;

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.ByDNI_NIEAndPassword(DNI_NIEToFind, Password), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(habitantToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }


            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIEAndPasswordIfPasswordIsIncorrect()
            {
                //Arrange
                var arrayOfHabitants = new List<PersistedHabitant>();
                arrayOfHabitants.Add(_dummyHabitant1);
                arrayOfHabitants.Add(_dummyHabitant2);
                _inMemoryOvMainDbContext.Habitants.AddRange(arrayOfHabitants);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var habitantToFind = _dummyHabitant1;
                var DNI_NIEToFind = habitantToFind.User.DNI_NIE;
                var Password = habitantToFind.User.Password.ToUpper();

                //Act
                var result = await _findHabitantDataService.FindAsync(HabitantFilter.ByDNI_NIEAndPassword(DNI_NIEToFind, Password), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }
        }
    }
}
