using ECondo.Application.Commands.PropertyOccupants.Delete;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.IntegrationTests.Commands.PropertyOccupants.Delete;

public class DeletePropertyOccupantCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DeletePropertyOccupantCommandHandler _handler;

    public DeletePropertyOccupantCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _handler = new DeletePropertyOccupantCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldDeleteOccupant_WhenOccupantExists()
    {
        // Arrange
        var occupantId = Guid.NewGuid();
        var propertyOccupant = new PropertyOccupant
        {
            Id = occupantId,
            PropertyId = Guid.NewGuid(),
            OccupantTypeId = Guid.NewGuid(),
            FirstName = "a",
            MiddleName = "a",
            LastName = "a",
        };

        _dbContext.PropertyOccupants.Add(propertyOccupant);
        await _dbContext.SaveChangesAsync();

        var command = new DeletePropertyOccupantCommand(
            occupantId
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var deletedOccupant = await _dbContext.PropertyOccupants.FirstOrDefaultAsync(po => po.Id == occupantId);
        deletedOccupant.Should().BeNull();
    }
    

    [Fact]
    public async Task Handle_ShouldLeaveDatabaseInConsistentState_AfterDeletion()
    {
        // Arrange
        var occupantId = Guid.NewGuid();
        
        var propertyOccupant = new PropertyOccupant
        {
            Id = occupantId,
            PropertyId = Guid.NewGuid(),
            OccupantTypeId = Guid.NewGuid(),
            FirstName = "a",
            MiddleName = "a",
            LastName = "a",
        };

        _dbContext.PropertyOccupants.Add(propertyOccupant);
        await _dbContext.SaveChangesAsync();

        var command = new DeletePropertyOccupantCommand(
            occupantId
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedOccupant = await _dbContext.PropertyOccupants.FirstOrDefaultAsync(po => po.Id == occupantId);
        deletedOccupant.Should().BeNull();
    }
}
