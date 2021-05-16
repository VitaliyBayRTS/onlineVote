using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.User.Create;
using OV.MainDb.User.Models.Public;
using System;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.User.Create
{
    public class CreateUserDataServiceTests
    {
        public class TestForCreateUser
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly ICreateUserDataService _createUserDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForCreateUser()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _createUserDataService = new CreateUserDataService(_inMemoryOvMainDbContextFactory);
            }

            [Fact]
            public async void ShouldCreateUser()
            {
                //Arrange
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = "FirstName",
                    SecondName = "SecondName",
                    SurName = "SurName",
                    SecondSurName = "SecondSurName",
                    Password = "Password",
                    DOB = DateTime.Now,
                    TblProvince_UID = 1,
                    Email = "Email",
                    PhoneNumber = "PhoneNumber"
                };

                //Act
                var result = await _createUserDataService.CreateAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(1);
                result.FirstName.Should().Be(candidate.FirstName);
                result.SecondName.Should().Be(candidate.SecondName);
                result.SurName.Should().Be(candidate.SurName);
                result.SecondSurName.Should().Be(candidate.SecondSurName);
                result.Email.Should().Be(candidate.Email);
                result.PhoneNumber.Should().Be(candidate.PhoneNumber);
                result.Password.Should().Be(candidate.Password);
                result.DOB.Should().Be(candidate.DOB);
            }

        }
    }
}
