using FluentAssertions;
using OV.MainDb.Configuration;
using OV.MainDb.UserElection.Find;
using OV.MainDb.UserElection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace OV.MainDb.Tests.UserElection.Find
{
    public class FindUserElectionServiceTests
    {
        static PersistedUserElection _dummyUserElection_1 = new PersistedUserElection
        {
            Id = 1,
            TblUser_UID = 1,
            TblElection_UID = 1
        };
        static PersistedUserElection _dummyUserElection_2 = new PersistedUserElection
        {
            Id = 2,
            TblUser_UID = 2,
            TblElection_UID = 2
        };


        public class TestForFindUserElection
        {
            private readonly IOvMainDbContextFactory _inMemoryOvMainDbContextFactory;
            private readonly IOvMainDbContext _inMemoryOvMainDbContext;
            private readonly IFindUserElectionService _findUserElectionService;
            private readonly CancellationToken cancellationToken = default;

            public TestForFindUserElection()
            {
                _inMemoryOvMainDbContextFactory = new InMemoryOvMainDbContextFactory(Guid.NewGuid().ToString());
                _inMemoryOvMainDbContext = _inMemoryOvMainDbContextFactory.Create();
                _findUserElectionService = new FindUserElectionService(new FindUserElectionDataService(_inMemoryOvMainDbContextFactory));

                _inMemoryOvMainDbContext.UserElections.Add(_dummyUserElection_1);
                _inMemoryOvMainDbContext.UserElections.Add(_dummyUserElection_2);
                _inMemoryOvMainDbContext.SaveChangesAsync(cancellationToken);
            }

            [Fact]
            public async void ShoudlFindUserElectionByIds()
            {
                //Arrange
                var userElectionToFind = _dummyUserElection_1;

                //Act
                var result = await _findUserElectionService.FindAsync(userElectionToFind.TblUser_UID, userElectionToFind.TblElection_UID, cancellationToken);

                //Assert
                result.Should().HaveCount(1);
                result.First().Id.Should().Be(userElectionToFind.Id);
                result.First().TblUser_UID.Should().Be(userElectionToFind.TblUser_UID);
                result.First().TblElection_UID.Should().Be(userElectionToFind.TblElection_UID);
            }

            [Fact]
            public async void ShoudlNotFindUserElectionIfUserIdIsWrong()
            {
                //Arrange
                var userElectionToFind = _dummyUserElection_1;
                var wrongTblUser_UID = 9999;

                //Act
                var result = await _findUserElectionService.FindAsync(wrongTblUser_UID, userElectionToFind.TblElection_UID, cancellationToken);

                //Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public async void ShoudlNotFindUserElectionIfElectionIdIsWrong()
            {
                //Arrange
                var userElectionToFind = _dummyUserElection_1;
                var wrongTblElection_UID = 9999;

                //Act
                var result = await _findUserElectionService.FindAsync(userElectionToFind.TblUser_UID, wrongTblElection_UID, cancellationToken);

                //Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public async void ShoudlNotFindUserElectionIfBothIdsAreWrong()
            {
                //Arrange
                var wrongTblUser_UID = 9999;
                var wrongTblElection_UID = 9999;

                //Act
                var result = await _findUserElectionService.FindAsync(wrongTblUser_UID, wrongTblElection_UID, cancellationToken);

                //Assert
                result.Should().BeEmpty();
            }
        }
    }
}
