using ECondo.Application.Commands.Profiles.Create;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Users;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.IntegrationTests.Commands.Profiles.Create;

public class CreateProfileCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly CreateProfileCommandHandler _handler;

    public CreateProfileCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _userContext = Substitute.For<IUserContext>();
        _handler = new CreateProfileCommandHandler(_userContext, _dbContext);
    }

    [Fact]
    public async Task Handle_ShouldCreateProfile_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "testuser",
            PhoneNumber = null
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new CreateProfileCommand(
            "John",
            "M",
            "Doe",
            "1234567890"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var profile = await _dbContext.UserDetails.FirstOrDefaultAsync(p => p.UserId == userId);
        profile.Should().NotBeNull();
        profile!.FirstName.Should().Be("John");
        profile.MiddleName.Should().Be("M");
        profile.LastName.Should().Be("Doe");

        var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        updatedUser.Should().NotBeNull();
        updatedUser!.PhoneNumber.Should().Be("1234567890");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userContext.UserId.Returns(userId);

        var command = new CreateProfileCommand(
            "John",
            "M",
            "Doe",
            "1234567890"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldSaveProfileAndUserDataCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "testuser",
            PhoneNumber = null
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new CreateProfileCommand(
            "Jane",
            "A",
            "Smith",
            "9876543210"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var profile = await _dbContext.UserDetails.FirstOrDefaultAsync(p => p.UserId == userId);
        profile.Should().NotBeNull();
        profile!.FirstName.Should().Be("Jane");
        profile.MiddleName.Should().Be("A");
        profile.LastName.Should().Be("Smith");

        var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        updatedUser.Should().NotBeNull();
        updatedUser!.PhoneNumber.Should().Be("9876543210");
    }
}
