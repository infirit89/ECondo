using ECondo.Application.Commands.PropertyOccupants.AddToProperty;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.IntegrationTests.Commands.PropertyOccupants.AddToProperty;

public class AddOccupantToPropertyCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly AddOccupantToPropertyCommandHandler _handler;

    public AddOccupantToPropertyCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        var publisher = Substitute.For<IPublisher>();
        _handler = new AddOccupantToPropertyCommandHandler(_dbContext, publisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenOccupantTypeDoesNotExist()
    {
        // Arrange
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Floor = "1",
            Number = "1"
        };

        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();

        var command = new AddOccupantToPropertyCommand(
            property.Id,
            "NonExistentType",
            "John",
            "M",
            "Doe",
            null,
            ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenDuplicateOccupantExists()
    {
        // Arrange
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Floor = "1",
            Number = "1",
        };
        var occupantType = new OccupantType { Id = Guid.NewGuid(), Name = "Tenant" };
        var existingOccupant = new PropertyOccupant
        {
            PropertyId = property.Id,
            Email = "test@example.com",
            OccupantTypeId = occupantType.Id,
            FirstName = "a",
            MiddleName = "a",
            LastName = "a",
        };

        _dbContext.Properties.Add(property);
        _dbContext.OccupantTypes.Add(occupantType);
        _dbContext.PropertyOccupants.Add(existingOccupant);
        await _dbContext.SaveChangesAsync();

        var command = new AddOccupantToPropertyCommand(
            property.Id,
            "Tenant",
            "John",
            "M",
            "Doe",
            "test@example.com",
            ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }
}
