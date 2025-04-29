using ECondo.Application.Commands.Properties.Create;
using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.Properties.Create;

public class CreatePropertyCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly CreatePropertyCommandHandler _handler;

    public CreatePropertyCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _handler = new CreatePropertyCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldCreateProperty_WhenEntranceAndPropertyTypeAreValid()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = buildingId,
            Number = "Entrance1"
        };

        var propertyType = new PropertyType
        {
            Id = Guid.NewGuid(),
            Name = "Apartment"
        };

        _dbContext.Entrances.Add(entrance);
        _dbContext.PropertyTypes.Add(propertyType);
        await _dbContext.SaveChangesAsync();

        var command = new CreatePropertyCommand(
            buildingId,
            "Entrance1",
            "Apartment",
            "2",
            "202",
            120,
            1
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var property = await _dbContext.Properties.FirstOrDefaultAsync(p => p.Number == "202");
        property.Should().NotBeNull();
        property!.Floor.Should().Be("2");
        property.BuiltArea.Should().Be(120);
        property.IdealParts.Should().Be(1);
        property.PropertyTypeId.Should().Be(propertyType.Id);
        property.EntranceId.Should().Be(entrance.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEntranceDoesNotExist()
    {
        // Arrange
        var command = new CreatePropertyCommand(
            Guid.NewGuid(),
            "NonExistentEntrance",
            "Apartment",
            "2",
            "202",
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
    public async Task Handle_ShouldReturnError_WhenPropertyTypeDoesNotExist()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = buildingId,
            Number = "Entrance1"
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        var command = new CreatePropertyCommand(
            buildingId,
            "Entrance1",
            "NonExistentType",
            "2",
            "202",
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
    public async Task Handle_ShouldReturnError_WhenPropertyAlreadyExists()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = buildingId,
            Number = "Entrance1"
        };

        var propertyType = new PropertyType
        {
            Id = Guid.NewGuid(),
            Name = "Apartment"
        };

        var existingProperty = new Property
        {
            EntranceId = entrance.Id,
            Number = "202",
            PropertyTypeId = propertyType.Id,
            Floor = "2",
            BuiltArea = 120,
            IdealParts = 1
        };

        _dbContext.Entrances.Add(entrance);
        _dbContext.PropertyTypes.Add(propertyType);
        _dbContext.Properties.Add(existingProperty);
        await _dbContext.SaveChangesAsync();

        var command = new CreatePropertyCommand(
            buildingId,
            "Entrance1",
            "Apartment",
            "2",
            "202",
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
