using ECondo.Application.Commands.PropertyOccupants.AcceptInvitation;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Users;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.IntegrationTests.Commands.PropertyOccupants.AcceptInvitation;

public class AcceptPropertyInvitationCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly AcceptPropertyInvitationCommandHandler _handler;

    public AcceptPropertyInvitationCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _userContext = Substitute.For<IUserContext>();
        _handler = new AcceptPropertyInvitationCommandHandler(_dbContext, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldAcceptInvitation_WhenValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "test@example.com" };
        var token = Guid.NewGuid();
        var propertyOccupant = new PropertyOccupant
        {
            Email = "test@example.com",
            InvitationToken = token,
            InvitationExpiresAt = DateTimeOffset.UtcNow.AddDays(1),
            InvitationStatus = InvitationStatus.Pending,
            FirstName = "a",
            MiddleName = "a",
            LastName = "a",
        };

        _dbContext.Users.Add(user);
        _dbContext.PropertyOccupants.Add(propertyOccupant);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new AcceptPropertyInvitationCommand(
            token,
            "test@example.com"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var updatedOccupant = await _dbContext.PropertyOccupants.FirstOrDefaultAsync(po => po.Email == "test@example.com");
        updatedOccupant.Should().NotBeNull();
        updatedOccupant!.UserId.Should().Be(userId);
        updatedOccupant.InvitationToken.Should().BeNull();
        updatedOccupant.InvitationExpiresAt.Should().BeNull();
        updatedOccupant.InvitationStatus.Should().Be(InvitationStatus.Accepted);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserEmailDoesNotMatch()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "wrong@example.com" };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var token = Guid.NewGuid();
        var command = new AcceptPropertyInvitationCommand(
            token,
            "test@example.com"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenInvitationNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "test@example.com" };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new AcceptPropertyInvitationCommand(
            Guid.Empty,
            "test@example.com"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenInvitationIsExpired()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "test@example.com" };
        var token = Guid.NewGuid();
        var propertyOccupant = new PropertyOccupant
        {
            Email = "test@example.com",
            InvitationToken = token,
            InvitationExpiresAt = DateTimeOffset.UtcNow.AddDays(-1),
            InvitationStatus = InvitationStatus.Pending,
            FirstName = "a",
            MiddleName = "a",
            LastName = "a",
        };

        _dbContext.Users.Add(user);
        _dbContext.PropertyOccupants.Add(propertyOccupant);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new AcceptPropertyInvitationCommand(
            token,
            "test@example.com"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }
}
