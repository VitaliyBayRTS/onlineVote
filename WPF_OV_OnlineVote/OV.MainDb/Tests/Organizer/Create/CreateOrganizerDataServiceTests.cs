using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Organizer.Create;
using OV.MainDb.Organizer.Models.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Organizer.Create
{
    public class CreateOrganizerDataServiceTests
    {
        public class TestForCreateOrganizer
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _ovMainDbContext;
            private readonly ICreateOrganizerDataService _createOrganizerDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForCreateOrganizer()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _ovMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _createOrganizerDataService = new CreateOrganizerDataService(_inMemoryOvMainDbContextFactory);
            }

            [Fact]
            public async void ShouldCreateOrganizer()
            {
                //Arrange
                CandidateOrganizer candidate = new CandidateOrganizer()
                {
                    tblUser_UID = 1,
                    tblElection_UID = 1,
                    ReferenceNumber = "RefNumber"
                };

                //Act
                var result = await _createOrganizerDataService.CreateAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                var organizers = _ovMainDbContext.Organizers.Where(o => true).ToList();
                organizers.Should().HaveCount(1);
            }

            [Fact]
            public async void ShouldCreateRangeOfOrganizers()
            {
                //Arrange
                List<CandidateOrganizer> candidates = new List<CandidateOrganizer>();
                candidates.Add(new CandidateOrganizer()
                {
                    tblUser_UID = 1,
                    tblElection_UID = 1,
                    ReferenceNumber = "RefNumber1"
                });
                candidates.Add(new CandidateOrganizer()
                {
                    tblUser_UID = 2,
                    tblElection_UID = 2,
                    ReferenceNumber = "RefNumber2"
                });
                candidates.Add(new CandidateOrganizer()
                {
                    tblUser_UID = 3,
                    tblElection_UID = 3,
                    ReferenceNumber = "RefNumbe1r"
                });

                //Act
                var result = await _createOrganizerDataService.CreateRangeAsync(candidates, cancellationToken);

                //Assert
                result.Should().BeTrue();
                var organizers = _ovMainDbContext.Organizers.Where(o => true).ToList();
                organizers.Should().HaveCount(candidates.Count);
            }

        }
    }
}
