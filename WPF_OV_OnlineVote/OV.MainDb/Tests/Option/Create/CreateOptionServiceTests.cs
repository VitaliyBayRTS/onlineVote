using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option;
using OV.MainDb.Option.Create;
using OV.MainDb.Option.Create.Models.Public;
using OV.MainDb.Option.Models.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Option.Create
{
    public class CreateOptionServiceTests
    {
        public class TestsForCreateOptionService
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly ICreateOptionService _createOptionService;
            private readonly ICreateOptionDataService _createOptionDataService;
            private readonly ICreateOptionValidator _validator;

            private readonly CancellationToken cancellationToken = default;

            public TestsForCreateOptionService()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();

                _validator = new CreateOptionValidator();
                _createOptionDataService = new CreateOptionDataService(_inMemoryOvMainDbContextFactory);

                _createOptionService = new CreateOptionService(_createOptionDataService, _validator);
            }


            [Fact]
            public async void ShouldCreateOption_Service()
            {
                //Arrange
                CandidateOption candidate = new CandidateOption()
                {
                    Name = "Name_Test",
                    Description = "Desc_Test",
                    tblElection_UID = 1
                };

                //Act
                var result = await _createOptionService.CreateAsync(candidate, cancellationToken);

                //Assert
                var isSuccess = result is CreateOptionSuccess;
                isSuccess.Should().BeTrue();
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfNameIsEmpty()
            {
                //Arrange
                CandidateOption candidate = new CandidateOption()
                {
                    Description = "Desc_Test",
                    tblElection_UID = 1
                };

                //Act
                var result = await _createOptionService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateOptionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.NameIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfDescriptionIsEmpty()
            {
                //Arrange
                CandidateOption candidate = new CandidateOption()
                {
                    Name = "Name_Test",
                    tblElection_UID = 1
                };

                //Act
                var result = await _createOptionService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateOptionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.DescriptionIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfElectionIdIsEmpty()
            {
                //Arrange
                CandidateOption candidate = new CandidateOption()
                {
                    Name = "Name_Test",
                    Description = "Desc_Test"
                };

                //Act
                var result = await _createOptionService.CreateAsync(candidate, cancellationToken);

                //Assert
                if (result is CreateOptionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.TblElection_UIDIsEmpty.ToString());
                }
            }

        }
    }
}
