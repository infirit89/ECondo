using ECondo.Application.Commands.Properties.Update;
using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.Properties.Update;

public class UpdatePropertyCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly UpdatePropertyCommandHandler _handler;

    public UpdatePropertyCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _handler = new UpdatePropertyCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPropertyTypeDoesNotExist()
    {
        // Arrange
        var property = new Property
        {
            Id = Guid.NewGuid(),
            EntranceId = Guid.NewGuid(),
            Number = "101",
            Floor = "1",
            BuiltArea = 100,
            IdealParts = 1,
            PropertyTypeId = Guid.NewGuid()
        };

        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();

        var command = new UpdatePropertyCommand(
            property.Id,
            "NonExistentType",
            "102",
            "2",
            120,
            1
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }


    [Fact]
    public async Task Handle_ShouldReturnError_WhenDuplicatePropertyExists()
    {
        // Arrange
        var propertyType = new PropertyType { Id = Guid.NewGuid(), Name = "Apartment" };
        var entrance = new Entrance { Id = Guid.NewGuid(), BuildingId = Guid.NewGuid(), Number = "1" };
        var existingProperty = new Property
        {
            Id = Guid.NewGuid(),
            EntranceId = entrance.Id,
            Number = "102",
            Floor = "2",
            BuiltArea = 120,
            IdealParts = 1,
            PropertyTypeId = propertyType.Id
        };

        var propertyToUpdate = new Property
        {
            Id = Guid.NewGuid(),
            EntranceId = entrance.Id,
            Number = "101",
            Floor = "1",
            BuiltArea = 100,
            IdealParts = 1,
            PropertyTypeId = propertyType.Id
        };

        _dbContext.PropertyTypes.Add(propertyType);
        _dbContext.Entrances.Add(entrance);
        _dbContext.Properties.AddRange(existingProperty, propertyToUpdate);
        await _dbContext.SaveChangesAsync();

        var command = new UpdatePropertyCommand(
            propertyToUpdate.Id,
            "Apartment",
            "102",
            "2",
            120,
            1
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }
}
