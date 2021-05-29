using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Organizer.Find;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MainDb.Organizer.Models;
using OV.MainDb.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Organizer.Find
{
    public class FindOrganizerDataServiceTests
    {
        static PersistedOrganizer _dummyOrganizer1 = new PersistedOrganizer()
        {
            Id = 1,
            tblUser_UID = 100,
            tblElection_UID = 1,
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
        static PersistedOrganizer _dummyOrganizer2 = new PersistedOrganizer()
        {
            Id = 2,
            tblUser_UID = 200,
            tblElection_UID = 1,
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
        public class TestForFindOrganizer
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindOrganizerDataService _findOrganizerDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindOrganizer()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findOrganizerDataService = new FindOrganizerDataService(_inMemoryOvMainDbContextFactory);

            }

            [Fact]
            public async void ShoudlFindAllOrganizers()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.All, cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfOrganizers.Count);
                result.All(h => h.User == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindHabitantById()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var IdToFind = organizerToFind.Id;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ById(IdToFind.Value), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(organizerToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByIdIfIdNoesNotExist()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var unexistingId = 1568;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ById(unexistingId), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }


            [Fact]
            public async void ShoudlFindHabitantByDNI_NIE()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var DNI_NIEToFind = organizerToFind.User.DNI_NIE;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByDNI_NIE(DNI_NIEToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(organizerToFind.Id);
                result.FirstOrDefault().User.Should().BeNull();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIEIfDNIE_NIE_DoesNotexist()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var DNI_NIEToFind = "58258369D";

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByDNI_NIE(DNI_NIEToFind), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlIncludeUser()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);


                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.All.AndIncludeUser(), cancellationToken);

                //Assert
                result.Should().HaveCount(arrayOfOrganizers.Count);
                result.All(h => h.User != null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlFindHabitantByDNI_NIE_PasswordReferenceNumber()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var DNI_NIEToFind = organizerToFind.User.DNI_NIE;
                var Password = organizerToFind.User.Password;
                var ReferenceNumber = organizerToFind.ReferenceNumber;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIEToFind, Password, ReferenceNumber), cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.FirstOrDefault().Id.Should().Be(organizerToFind.Id);
                result.FirstOrDefault().ReferenceNumber.Should().Be(organizerToFind.ReferenceNumber);
                result.FirstOrDefault().User.Should().BeNull();
            }


            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIE_PasswordRefenceNumberIfPasswordIsIncorrect()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var DNI_NIEToFind = organizerToFind.User.DNI_NIE;
                var ReferenceNumber = organizerToFind.ReferenceNumber;
                var Password = organizerToFind.User.Password.ToUpper();

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIEToFind, Password, ReferenceNumber), 
                                                                        cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlNotFindHabitantByDNI_NIE_PasswordRefenceNumberIfReferenceNumberIsIncorrect()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var DNI_NIEToFind = organizerToFind.User.DNI_NIE;
                var ReferenceNumber = "WrongReferenceNumber";
                var Password = organizerToFind.User.Password;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIEToFind, Password, ReferenceNumber), 
                                                                        cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlFindHabitantByElectionId()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var tblElection_UID = organizerToFind.tblElection_UID;
                var countOfOrganizersWithTheSameElectionId = arrayOfOrganizers.Select(o => o.tblElection_UID == tblElection_UID).ToList().Count;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByElectionId(tblElection_UID), cancellationToken);

                //Assert
                result.Should().HaveCount(countOfOrganizersWithTheSameElectionId);
                result.Where(u => u.Id == organizerToFind.Id).Should().HaveCount(1);
                result.All(u => u.User == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByElectionIdIfElectionIdIsWrong()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var unexistingElectionId = 12245;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByElectionId(unexistingElectionId), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }

            [Fact]
            public async void ShoudlFindHabitantByReferenceNumber()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var organizerToFind = _dummyOrganizer1;
                var referenceNumber = organizerToFind.ReferenceNumber;
                var countOfOrganizersWithTheSamereferenceNumber = arrayOfOrganizers.Where(o => string.Equals(o.ReferenceNumber, referenceNumber, StringComparison.Ordinal)).ToList().Count;

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByReferenceNumber(referenceNumber), cancellationToken);

                //Assert
                result.Should().HaveCount(countOfOrganizersWithTheSamereferenceNumber);
                result.Where(u => u.Id == organizerToFind.Id).Should().HaveCount(1);
                result.All(u => u.User == null).Should().BeTrue();
            }

            [Fact]
            public async void ShoudlNotFindHabitantByReferenceNumberIfReferenceNumberIsWrong()
            {
                //Arrange
                var arrayOfOrganizers = new List<PersistedOrganizer>();
                arrayOfOrganizers.Add(_dummyOrganizer1);
                arrayOfOrganizers.Add(_dummyOrganizer2);
                _inMemoryOvMainDbContext.Organizers.AddRange(arrayOfOrganizers);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                var unexistingReferenceNumber = "unexistingReferenceNumber";

                //Act
                var result = await _findOrganizerDataService.FindAsync(OrganizerFilter.ByReferenceNumber(unexistingReferenceNumber), cancellationToken);

                //Assert
                result.Should().HaveCount(0);
            }
        }
    }
}
