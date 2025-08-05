using ECondo.Application.Commands.PropertyOccupants.Update;
using ECondo.Application.Events.PropertyOccupant;
using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.PropertyOccupants.Update;

#if false
public class UpdatePropertyOccupantCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly UpdatePropertyOccupantCommandHandler _handler;

    public UpdatePropertyOccupantCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _publisher = Substitute.For<IPublisher>();
        _handler = new UpdatePropertyOccupantCommandHandler(_dbContext, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldUpdateOccupant_WhenValid()
    {
        // Arrange
        var occupantType = new OccupantType { Id = Guid.NewGuid(), Name = "Tenant" };
        var occupant = new PropertyOccupant
        {
            Id = Guid.NewGuid(),
            FirstName = "OldFirstName",
            MiddleName = "OldMiddleName",
            LastName = "OldLastName",
            Email = "old@example.com",
            OccupantTypeId = occupantType.Id
        };

        _dbContext.OccupantTypes.Add(occupantType);
        _dbContext.PropertyOccupants.Add(occupant);
        await _dbContext.SaveChangesAsync();

        var command = new UpdatePropertyOccupantCommand(
            occupant.Id,
            "NewFirstName",
            "NewMiddleName",
            "NewLastName",
            "Tenant",
            "old@example.com",
            ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var updatedOccupant = await _dbContext.PropertyOccupants.FirstOrDefaultAsync(po => po.Id == occupant.Id);
        updatedOccupant.Should().NotBeNull();
        updatedOccupant!.FirstName.Should().Be("NewFirstName");
        updatedOccupant.MiddleName.Should().Be("NewMiddleName");
        updatedOccupant.LastName.Should().Be("NewLastName");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenOccupantTypeDoesNotExist()
    {
        // Arrange
        var occupant = new PropertyOccupant
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            MiddleName = "MiddleName",
            LastName = "LastName",
            Email = "test@example.com",
            OccupantTypeId = Guid.NewGuid()
        };

        _dbContext.PropertyOccupants.Add(occupant);
        await _dbContext.SaveChangesAsync();

        var command = new UpdatePropertyOccupantCommand(
            occupant.Id,
            "NewFirstName",
            "NewMiddleName",
            "NewLastName",
            "NonExistentType",
            "test@example.com",
            ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }
    

    [Fact]
    public async Task Handle_ShouldSendInvitation_WhenEmailIsUpdated()
    {
        // Arrange
        var occupantType = new OccupantType { Id = Guid.NewGuid(), Name = "Tenant" };
        var occupant = new PropertyOccupant
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            MiddleName = "MiddleName",
            LastName = "LastName",
            Email = "old@example.com",
            OccupantTypeId = occupantType.Id
        };

        _dbContext.OccupantTypes.Add(occupantType);
        _dbContext.PropertyOccupants.Add(occupant);
        await _dbContext.SaveChangesAsync();

        var command = new UpdatePropertyOccupantCommand(
            occupant.Id,
            "FirstName",
            "MiddleName",
            "LastName",
            "Tenant",
            "new@example.com",
            "https://example.com/invite"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var updatedOccupant = await _dbContext.PropertyOccupants.FirstOrDefaultAsync(po => po.Id == occupant.Id);
        updatedOccupant.Should().NotBeNull();
        updatedOccupant!.Email.Should().Be("new@example.com");
        updatedOccupant.InvitationToken.Should().NotBeNull();
        updatedOccupant.InvitationStatus.Should().Be(InvitationStatus.Pending);

        await _publisher.Received(1).Publish(
            Arg.Is<OccupantInvitedEvent>(e =>
                e.Email == "new@example.com" &&
                e.FirstName == "FirstName" &&
                e.ReturnUri == "https://example.com/invite"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldClearInvitationDetails_WhenEmailIsRemoved()
    {
        // Arrange
        var occupantType = new OccupantType { Id = Guid.NewGuid(), Name = "Tenant" };
        var occupant = new PropertyOccupant
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            MiddleName = "MiddleName",
            LastName = "LastName",
            Email = "old@example.com",
            InvitationToken = Guid.NewGuid(),
            InvitationExpiresAt = DateTimeOffset.UtcNow.AddHours(48),
            InvitationSentAt = DateTimeOffset.UtcNow,
            InvitationStatus = InvitationStatus.Pending,
            OccupantTypeId = occupantType.Id
        };

        _dbContext.OccupantTypes.Add(occupantType);
        _dbContext.PropertyOccupants.Add(occupant);
        await _dbContext.SaveChangesAsync();

        var command = new UpdatePropertyOccupantCommand(
            occupant.Id,
            "FirstName",
            "MiddleName",
            "LastName",
            "Tenant",
            null,
            ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var updatedOccupant = await _dbContext.PropertyOccupants.FirstOrDefaultAsync(po => po.Id == occupant.Id);
        updatedOccupant.Should().NotBeNull();
        updatedOccupant!.Email.Should().BeNull();
        updatedOccupant.InvitationToken.Should().BeNull();
        updatedOccupant.InvitationStatus.Should().Be(InvitationStatus.NotInvited);
    }
}
#endif
