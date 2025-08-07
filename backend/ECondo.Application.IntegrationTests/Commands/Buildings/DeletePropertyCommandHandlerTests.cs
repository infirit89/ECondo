using ECondo.Application.Commands.Properties.Delete;
using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.Properties.Delete;

public class DeletePropertyCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DeletePropertyCommandHandler _handler;

    public DeletePropertyCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _handler = new DeletePropertyCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldDeletePropertyAndOccupants_WhenPropertyExists()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var property = new Property
        {
            Id = propertyId,
            Floor = "1",
            Number = "1",
            PropertyOccupants = new HashSet<PropertyOccupant>
            {
                new PropertyOccupant
                {
                    PropertyId = propertyId,
                    FirstName = "a",
                    MiddleName = "a",
                    LastName = "a",
                },
                new PropertyOccupant
                {
                    PropertyId = propertyId,
                    FirstName = "a",
                    MiddleName = "a",
                    LastName = "a",
                }
            }
        };

        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();

        var command = new DeletePropertyCommand(
            propertyId
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        var deletedProperty = await _dbContext.Properties.FirstOrDefaultAsync(p => p.Id == propertyId);
        deletedProperty.Should().BeNull();
        var remainingOccupants = await _dbContext.PropertyOccupants.Where(po => po.PropertyId == propertyId).ToListAsync();
        remainingOccupants.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPropertyDoesNotExist()
    {
        // Arrange
        var command = new DeletePropertyCommand(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldLeaveDatabaseInConsistentState_AfterDeletion()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var property = new Property
        {
            Id = propertyId,
            Floor = "1",
            Number = "1",
            PropertyOccupants = new HashSet<PropertyOccupant>
            {
                new PropertyOccupant
                {
                    PropertyId = propertyId,
                    FirstName = "a",
                    MiddleName = "a",
                    LastName = "a",
                },
                new PropertyOccupant
                {
                    PropertyId = propertyId,
                    FirstName = "a",
                    MiddleName = "a",
                    LastName = "a",
                }
            }
        };

        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();

        var command = new DeletePropertyCommand(
            propertyId
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedProperty = await _dbContext.Properties.FirstOrDefaultAsync(p => p.Id == propertyId);
        deletedProperty.Should().BeNull();
        var remainingOccupants = await _dbContext.PropertyOccupants.Where(po => po.PropertyId == propertyId).ToListAsync();
        remainingOccupants.Should().BeEmpty();
    }
}
