using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Models;
using OV.MainDb.Option.Modify;
using OV.MainDb.Option.Modify.Models.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OV.MainDb.Tests.Option.Modify
{
    public class ModifyOptionDataServiceTests
    {
        static PersistedOption _dummyOption_1 = new PersistedOption()
        {
            Id = 1,
            Name = "nameTest",
            Description = "DescTest",
            Votes = default(int),
            tblElection_UID = 1
        };

        public class TestForModifyOption
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IModifyOptionDataService _modifyOptionDataService;
            private readonly CancellationToken cancellationToken = default;

            public TestForModifyOption()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _modifyOptionDataService = new ModifyOptionDataService(_inMemoryOvMainDbContextFactory);
            }


            [Fact]
            public async void ShouldModifyOption()
            {
                //Arrange
                var optionToModify = _dummyOption_1;
                _inMemoryOvMainDbContext.Options.Add(optionToModify);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                ModifyOptionCandidate candidate = new ModifyOptionCandidate()
                {
                    Id = optionToModify.Id.Value,
                    Name = "E_Name",
                    Description = "E_Desc"
                };

                //Act
                var result = await _modifyOptionDataService.ModifyAsync(candidate, cancellationToken);

                //Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(candidate.Id);
                result.Name.Should().Be(candidate.Name);
                result.Description.Should().Be(candidate.Description);
                result.tblElection_UID.Should().Be(optionToModify.tblElection_UID);
            }

            [Fact]
            public async void ShouldNotModifyOptionIfIdDoesNotExist()
            {
                //Arrange
                _inMemoryOvMainDbContext.Options.Add(_dummyOption_1);
                await _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);

                ModifyOptionCandidate candidate = new ModifyOptionCandidate()
                {
                    Id = 999,
                    Name = "E_Name",
                    Description = "E_Desc"
                };

                //Act
                Func<Task> act = async () => await _modifyOptionDataService.ModifyAsync(candidate, cancellationToken);

                //Assert
                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }
    }
}
