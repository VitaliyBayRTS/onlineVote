using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Create;
using OV.MainDb.Option.Models.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Option.Create
{
    public class CreateOptionDataServiceTests
    {
        public class TestsForCreateOption
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly ICreateOptionDataService _createOptionDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestsForCreateOption()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _createOptionDataService = new CreateOptionDataService(_inMemoryOvMainDbContextFactory);
            }

            [Fact]
            public async void ShouldCreateOption()
            {
                //Arrange
                CandidateOption candidate = new CandidateOption()
                {
                    Name = "Name_Test",
                    Description = "Desc_Test",
                    tblElection_UID = 1
                };

                //Act
                var result = await _createOptionDataService.CreateAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(1);
                result.Name.Should().Be(candidate.Name);
                result.Description.Should().Be(candidate.Description);
                result.tblElection_UID.Should().Be(candidate.tblElection_UID);
            }
        }
    }
}
