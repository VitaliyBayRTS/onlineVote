using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option;
using OV.MainDb.Option.Models;
using OV.MainDb.Option.Modify;
using OV.MainDb.Option.Modify.Models.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.Option.Modify
{
    public class ModifyOptionServiceTests
    {
        public class TestsForModifyOption_Service
        {
            private PersistedOption optionToModify;

            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;

            private readonly IModifyOptionDataService _modifyOptionDataService;
            private readonly IModifyOptionValidator _candidateOptionValidator;
            private readonly IModifyOptionService _modifyOptionService;

            private readonly CancellationToken cancellationToken = default;

            public TestsForModifyOption_Service()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();


                _candidateOptionValidator = new ModifyOptionValidator();
                _modifyOptionDataService = new ModifyOptionDataService(_inMemoryOvMainDbContextFactory);

                _modifyOptionService = new ModifyOptionService(_modifyOptionDataService, _candidateOptionValidator);


                optionToModify = new PersistedOption()
                {
                    Id = 1,
                    Name = "OriginalName",
                    Description = "OriginalDesc",
                    tblElection_UID = 1,
                    Votes = default(int)
                };

                _inMemoryOvMainDbContext.Options.Add(optionToModify);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
            }

            [Fact]
            public async void ShouldModifyOption_Service()
            {
                //Arrange
                ModifyOptionCandidate candidate = new ModifyOptionCandidate()
                {
                    Id = optionToModify.Id.Value,
                    Name = "E_Name",
                    Description = "E_Desc"
                };

                //Act
                var result = await _modifyOptionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                var isSuccess = result is ModifyOptionSuccess;
                isSuccess.Should().BeTrue();
            }


            [Fact]
            public async void ShouldReturnFailureResponseIfNameIsEmpty()
            {
                //Arrange
                ModifyOptionCandidate candidate = new ModifyOptionCandidate()
                {
                    Id = optionToModify.Id.Value,
                    Description = "E_Desc",
                };

                //Act
                var result = await _modifyOptionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyOptionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.NameIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfDescriptionIsEmpty()
            {
                //Arrange
                ModifyOptionCandidate candidate = new ModifyOptionCandidate()
                {
                    Id = optionToModify.Id.Value,
                    Name = "Name_Test"
                };

                //Act
                var result = await _modifyOptionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyOptionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.DescriptionIsEmpty.ToString());
                }
            }

            [Fact]
            public async void ShouldReturnFailureResponseIfOptionIdIsEmpty()
            {
                //Arrange
                ModifyOptionCandidate candidate = new ModifyOptionCandidate()
                {
                    Name = "Name_Test",
                    Description = "E_Desc",
                };

                //Act
                var result = await _modifyOptionService.ModifyAsync(candidate, cancellationToken);

                //Assert
                if (result is ModifyOptionFailure failure)
                {
                    failure.FailureReasons[0].Code.ToString().Should().Be(OptionFailureReason.IdIsEmpty.ToString());
                }
            }
        }
    }
}
