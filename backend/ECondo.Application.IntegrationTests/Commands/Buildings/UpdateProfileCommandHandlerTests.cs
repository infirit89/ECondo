using ECondo.Application.Commands.Profiles.Update;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.IntegrationTests.Commands.Profiles.Update;

public class UpdateProfileCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly UpdateProfileCommandHandler _handler;

    public UpdateProfileCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _userContext = Substitute.For<IUserContext>();
        _handler = new UpdateProfileCommandHandler(_dbContext, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldUpdateProfile_WhenProfileExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = new ProfileDetails
        {
            UserId = userId,
            FirstName = "OldFirstName",
            MiddleName = "OldMiddleName",
            LastName = "OldLastName"
        };

        _dbContext.UserDetails.Add(profile);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new UpdateProfileCommand(
            "NewFirstName",
            "NewMiddleName",
            "NewLastName"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var updatedProfile = await _dbContext.UserDetails.FirstOrDefaultAsync(p => p.UserId == userId);
        updatedProfile.Should().NotBeNull();
        updatedProfile!.FirstName.Should().Be("NewFirstName");
        updatedProfile.MiddleName.Should().Be("NewMiddleName");
        updatedProfile.LastName.Should().Be("NewLastName");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProfileDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userContext.UserId.Returns(userId);

        var command = new UpdateProfileCommand(
            "NewFirstName",
            "NewMiddleName",
            "NewLastName"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldSaveUpdatedProfileCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = new ProfileDetails
        {
            UserId = userId,
            FirstName = "InitialFirstName",
            MiddleName = "InitialMiddleName",
            LastName = "InitialLastName"
        };

        _dbContext.UserDetails.Add(profile);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new UpdateProfileCommand(
            "UpdatedFirstName",
            "UpdatedMiddleName",
            "UpdatedLastName"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedProfile = await _dbContext.UserDetails.FirstOrDefaultAsync(p => p.UserId == userId);
        updatedProfile.Should().NotBeNull();
        updatedProfile!.FirstName.Should().Be("UpdatedFirstName");
        updatedProfile.MiddleName.Should().Be("UpdatedMiddleName");
        updatedProfile.LastName.Should().Be("UpdatedLastName");
    }
}
