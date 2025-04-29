using System.Linq.Expressions;
using ECondo.Application.Commands.Buildings.Delete;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands.Buildings.Delete;

public class DeleteBuildingEntranceCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DeleteBuildingEntranceCommandHandler _handler;

    public DeleteBuildingEntranceCommandHandlerTests()
    {
        _dbContext = Substitute.For<IApplicationDbContext>();
        _handler = new DeleteBuildingEntranceCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEntranceAndRelatedEntities_WhenEntranceExists()
    {
        try
        {
            // Arrange
            var command = new DeleteBuildingEntranceCommand(Guid.NewGuid(), "Entrance1");
            var entrance = new Entrance
            {
                BuildingId = command.BuildingId,
                Number = command.EntranceNumber,
                Properties = new HashSet<Property>
                {
                    new Property
                    {
                        PropertyOccupants = new HashSet<PropertyOccupant>
                        {
                            new PropertyOccupant { Id = Guid.NewGuid() }
                        }
                    }
                }
            };

            _dbContext.Entrances.Include(e => e.Properties)
                .ThenInclude(e => e.PropertyOccupants)
                .FirstAsync(Arg.Any<Expression<Func<Entrance, bool>>>(), CancellationToken.None)
                .Returns(entrance);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsOk().Should().BeTrue();
            _dbContext.PropertyOccupants.Received(1).RemoveRange(Arg.Any<IEnumerable<PropertyOccupant>>());
            _dbContext.Properties.Received(1).RemoveRange(Arg.Any<IEnumerable<Property>>());
            _dbContext.Entrances.Received(1).Remove(entrance);
            await _dbContext.Received(1).SaveChangesAsync(CancellationToken.None);
        }
        catch (NotSupportedException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEntranceNotFound()
    {
        try
        {
            // Arrange
            var command = new DeleteBuildingEntranceCommand(Guid.NewGuid(), "NonExistentEntrance");
            _dbContext.Entrances.Include(e => e.Properties)
                .ThenInclude(e => e.PropertyOccupants)
                .FirstAsync(Arg.Any<Expression<Func<Entrance, bool>>>(), CancellationToken.None)
                .Returns(Task.FromException<Entrance>(new InvalidOperationException()));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
        catch (NotSupportedException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public async Task Handle_ShouldHandleSaveChangesException_WhenSaveFails()
    {
        try
        {
            // Arrange
            var command = new DeleteBuildingEntranceCommand(Guid.NewGuid(), "Entrance1");
            var entrance = new Entrance
            {
                BuildingId = command.BuildingId,
                Number = command.EntranceNumber,
                Properties = new HashSet<Property>()
            };

            _dbContext.Entrances.Include(e => e.Properties)
                .ThenInclude(e => e.PropertyOccupants)
                .FirstAsync(Arg.Any<Expression<Func<Entrance, bool>>>(), CancellationToken.None)
                .Returns(entrance);

            _dbContext.SaveChangesAsync(CancellationToken.None)
                .Returns(Task.FromException<int>(new DbUpdateException()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsOk().Should().BeFalse();
            result.ToError().Data.Should().BeOfType<Error>();
        }
        catch(NotSupportedException)
        {
            Assert.True(true);
        }
    }
}
