using ECondo.Application.Commands.Buildings.Delete;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.IntegrationTests.Commands.Buildings.Delete;

public class DeleteBuildingEntranceCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DeleteBuildingEntranceCommandHandler _handler;

    public DeleteBuildingEntranceCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<Infrastructure.Contexts.ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new Infrastructure.Contexts.ECondoDbContext(options);
        _handler = new DeleteBuildingEntranceCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEntranceAndRelatedEntities_WhenEntranceExists()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = buildingId,
            Number = "Entrance1",
            Properties = new HashSet<Property>
            {
                new Property
                {
                    EntranceId = Guid.NewGuid(),
                    Floor = "1",
                    Number = "1",
                    PropertyOccupants = new HashSet<PropertyOccupant>
                    {
                        new PropertyOccupant()
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "A",
                            MiddleName = "A",
                            LastName = "A",
                        }
                    }
                }
            }
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteBuildingEntranceCommand(buildingId, "Entrance1");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsOk());
        Assert.False(await _dbContext.Entrances.AnyAsync(e => e.Id == entrance.Id));
        Assert.False(await _dbContext.Properties.AnyAsync());
        Assert.False(await _dbContext.PropertyOccupants.AnyAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEntranceNotFound()
    {
        // Arrange
        var command = new DeleteBuildingEntranceCommand(Guid.NewGuid(), "NonExistentEntrance");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldLeaveDatabaseInConsistentState_AfterDeletion()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = buildingId,
            Number = "Entrance1",
            Properties = new HashSet<Property>
            {
                new Property
                {
                    EntranceId = Guid.NewGuid(),
                    Floor = "1",
                    Number = "1",
                    PropertyOccupants = new HashSet<PropertyOccupant>
                    {
                        new PropertyOccupant
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "A",
                            MiddleName = "A",
                            LastName = "A",
                        }
                    }
                }
            }
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteBuildingEntranceCommand(buildingId, "Entrance1");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(await _dbContext.Entrances.AnyAsync());
        Assert.False(await _dbContext.Properties.AnyAsync());
        Assert.False(await _dbContext.PropertyOccupants.AnyAsync());
    }
}
