using FluentAssertions;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Find;
using OV.MainDb.Province.Models;
using OV.MainDb.User;
using OV.MainDb.User.Create;
using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Find;
using OV.MainDb.User.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.User.Create
{
    public class CreateUserServiceTests
    {
        public class TestForCreateUserService
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly IFindAutonomousCommunityDataService _findAutonomousCommunityDataService;
            private readonly IFindProvinceDataService _findProvinceDataService;
            private readonly ICreateUserService _createUserService;
            private readonly ICreateUserDataService _createUserDataService;
            private readonly ICandidateUserValidator _validator;

            private readonly CancellationToken cancellationToken = default;

            public TestForCreateUserService()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();

                _findAutonomousCommunityDataService = new FindAutonomousCommunityDataService(_inMemoryOvMainDbContext);
                _findProvinceDataService = new FindProvinceDataService(_inMemoryOvMainDbContext);

                _validator = new CandidateUserValidator(_findAutonomousCommunityDataService, _findProvinceDataService, new FindUserDataService(_inMemoryOvMainDbContextFactory));
                _createUserDataService = new CreateUserDataService(_inMemoryOvMainDbContextFactory);

                _createUserService = new CreateUserService(_createUserDataService, _validator);
            }

            [Fact]
            public async void ShouldCreateUser()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Id = 1,
                    Name = "NameAC",
                    Provinces = new List<PersistedProvince>()
                    {
                        new PersistedProvince()
                        {
                            Id = 1,
                            Name = "NameProvince",
                            tblAutonomousCommunity_UID = 1
                        }
                    }
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName",
                    SurName = "SurName",
                    SecondSurName = "SecondSurName",
                    Password = "Password",
                    DOB = DateTime.Now,
                    TblProvince_UID = autonomousCommunity.Provinces.FirstOrDefault().Id,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isSuccess = result is CreateUserSuccess;
                isSuccess.Should().BeTrue();
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfFirstNameIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                PersistedProvince province = new PersistedProvince()
                {
                    Name = "NameProvince",
                    tblAutonomousCommunity_UID = autonomousCommunity.Id
                };
                _inMemoryOvMainDbContext.Provinces.Add(province);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = null,
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    DOB = DateTime.Now,

                    TblProvince_UID = province.Id,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.FirstNameIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfFirstSurNameIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                PersistedProvince province = new PersistedProvince()
                {
                    Name = "NameProvince",
                    tblAutonomousCommunity_UID = autonomousCommunity.Id
                };
                _inMemoryOvMainDbContext.Provinces.Add(province);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = null,
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    DOB = DateTime.Now,
                    TblProvince_UID = province.Id,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.FirstSurNameIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfPasswordIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                PersistedProvince province = new PersistedProvince()
                {
                    Name = "NameProvince",
                    tblAutonomousCommunity_UID = autonomousCommunity.Id
                };
                _inMemoryOvMainDbContext.Provinces.Add(province);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = null,
                    DOB = DateTime.Now,

                    TblProvince_UID = province.Id,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.PasswordIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfDateOfBirthIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                PersistedProvince province = new PersistedProvince()
                {
                    Name = "NameProvince",
                    tblAutonomousCommunity_UID = autonomousCommunity.Id
                };
                _inMemoryOvMainDbContext.Provinces.Add(province);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    TblProvince_UID = province.Id,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.DateOfBirthIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfProvinceIdIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    DOB = DateTime.Now,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.ProvinceIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfEmailIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                PersistedProvince province = new PersistedProvince()
                {
                    Name = "NameProvince",
                    tblAutonomousCommunity_UID = autonomousCommunity.Id
                };
                _inMemoryOvMainDbContext.Provinces.Add(province);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    DOB = DateTime.Now,
                    TblProvince_UID = province.Id,
                    Email = null,
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.EmailIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfPhoneNumberIsEmpty()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                PersistedProvince province = new PersistedProvince()
                {
                    Name = "NameProvince",
                    tblAutonomousCommunity_UID = autonomousCommunity.Id
                };
                _inMemoryOvMainDbContext.Provinces.Add(province);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    DOB = DateTime.Now,
                    TblProvince_UID = province.Id,
                    Email = "Email",
                    PhoneNumber = null
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.PhoneNumberIsEmpty.ToString());
                }
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfProvinceIdDoeNotExist()
            {
                //Arrange
                PersistedAutonomousCommunity autonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Name = "NameAC2"
                };
                _inMemoryOvMainDbContext.AutonomousCommunities.Add(autonomousCommunity);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName", // Can be null
                    SurName = "SurName",
                    SecondSurName = "SecondSurName", // Can be null
                    Password = "Password",
                    DOB = DateTime.Now,

                    TblProvince_UID = 1,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                ICreateUserResponse result = await _createUserService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isFailure = result is CreateUserFailure;
                isFailure.Should().BeTrue();
                if (result is CreateUserFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(UserFailureReason.ProvinceDoesNotExist.ToString());
                }
            }
        }
    }
}
