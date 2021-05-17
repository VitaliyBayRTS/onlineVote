using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.SuperAdmin.Find;
using OV.MainDb.SuperAdmin.Find.Models.Public;
using OV.MainDb.SuperAdmin.Models;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.SuperAdmin.Find
{
    public class FindSuperAdminDataServiceTests
    {
        static PersistedSuperAdmin _dummySuperAdmin1 = new PersistedSuperAdmin()
        {
            Id = 1,
            tblUser_UID = 100,
            ReferenceNumber = "refNumber1",
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
        static PersistedSuperAdmin _dummySuperAdmin2 = new PersistedSuperAdmin()
        {
            Id = 2,
            tblUser_UID = 200,
            ReferenceNumber = "refNumber2",
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
        public class TestForFindSuperAdmin
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindSuperAdminDataService _findSuperAdminDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindSuperAdmin()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findSuperAdminDataService = new FindSuperAdminDataService(_inMemoryOvMainDbContextFactory);

            }

            [Fact]
            public async void ShoudlFindAllSuperAdmin()
            {
                //Arrange
                var arrayOfSuperAdmins = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmins.Add(_dummySuperAdmin1);
                arrayOfSuperAdmins.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmins);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfSuperAdmins.Count);
                result.All(h => h.User == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindHabitantById()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var superAdminToFind = _dummySuperAdmin1;
                var IdToFind = superAdminToFind.Id;

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ById(IdToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(superAdminToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByIdIfIdNoesNotExist()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var unexistingId = 1568;

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ById(unexistingId), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }


            [Fact]
            public async void ShoudlFindHabitantByDNI_NIE()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var superAdminToFind = _dummySuperAdmin1;
                var DNI_NIEToFind = superAdminToFind.User.DNI_NIE;

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ByDNI_NIE(DNI_NIEToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(superAdminToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIEIfDNIE_NIE_DoesNotexist()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var DNI_NIEToFind = "58258369D";

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ByDNI_NIE(DNI_NIEToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlIncludeUser()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);


                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.All.AndIncludeUser(), cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfSuperAdmin.Count);
                result.All(h => h.User != null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindHabitantByDNI_NIE_PasswordReferenceNumber()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var superAdminToFind = _dummySuperAdmin1;
                var DNI_NIEToFind = superAdminToFind.User.DNI_NIE;
                var Password = superAdminToFind.User.Password;
                var ReferenceNumber = superAdminToFind.ReferenceNumber;

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIEToFind, Password, ReferenceNumber), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(superAdminToFind.Id);
                result.FirstOrDefault().ReferenceNumber.Should().Be(superAdminToFind.ReferenceNumber);
                result.FirstOrDefault().User.Should().BeNull();
            }


            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIE_PasswordRefenceNumberIfPasswordIsIncorrect()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var superAdminToFind = _dummySuperAdmin1;
                var DNI_NIEToFind = superAdminToFind.User.DNI_NIE;
                var ReferenceNumber = superAdminToFind.ReferenceNumber;
                var Password = superAdminToFind.User.Password.ToUpper();

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIEToFind, Password, ReferenceNumber),
                                                                        cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIE_PasswordRefenceNumberIfReferenceNumberIsIncorrect()
            {
                //Arrange
                var arrayOfSuperAdmin = new List<PersistedSuperAdmin>();
                arrayOfSuperAdmin.Add(_dummySuperAdmin1);
                arrayOfSuperAdmin.Add(_dummySuperAdmin2);
                _inMemoryOvMainDbContext.SuperAdmin.AddRange(arrayOfSuperAdmin);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var superAdminToFind = _dummySuperAdmin1;
                var DNI_NIEToFind = superAdminToFind.User.DNI_NIE;
                var ReferenceNumber = "WrongReferenceNumber";
                var Password = superAdminToFind.User.Password;

                //Act
                var result = await _findSuperAdminDataService.FindAsync(SuperAdminFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIEToFind, Password, ReferenceNumber),
                                                                        cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }
        }
    }
}
