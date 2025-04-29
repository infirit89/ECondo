using ECondo.Application.Commands.Identity.GenerateAccessToken;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity.GenerateAccessToken;

public class GenerateAccessTokenCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthTokenService _authService;
    private readonly GenerateAccessTokenCommandHandler _handler;

    public GenerateAccessTokenCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);
        _authService = Substitute.For<IAuthTokenService>();
        _handler = new GenerateAccessTokenCommandHandler(_userManager, _authService);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidRefreshTokenError_WhenTokenIsNullOrEmpty()
    {
        // Arrange
        var command = new GenerateAccessTokenCommand(string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.InvalidRefreshToken");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidRefreshTokenError_WhenTokenDoesNotExist()
    {
        // Arrange
        var command = new GenerateAccessTokenCommand("nonexistent-token");
        _authService.GetRefreshTokenAsync(command.RefreshToken).Returns((RefreshToken?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.InvalidRefreshToken");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidUserError_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new GenerateAccessTokenCommand("valid-token");
        var refreshToken = new RefreshToken { Value = command.RefreshToken, UserId = Guid.NewGuid() };
        _authService.GetRefreshTokenAsync(command.RefreshToken).Returns(refreshToken);
        _userManager.FindByIdAsync(refreshToken.UserId.ToString()).Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAccessTokenIsGeneratedSuccessfully()
    {
        // Arrange
        var command = new GenerateAccessTokenCommand("valid-token");
        var refreshToken = new RefreshToken { Value = command.RefreshToken, UserId = Guid.NewGuid() };
        var user = new User { Id = refreshToken.UserId };
        var accessToken = new AccessToken { Value = "access-token", MinutesExpiry = 60 };

        _authService.GetRefreshTokenAsync(command.RefreshToken).Returns(refreshToken);
        _userManager.FindByIdAsync(refreshToken.UserId.ToString()).Returns(user);
        _authService.GenerateAccessTokenAsync(user).Returns(accessToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        result.ToSuccess().Data.Should().NotBeNull();
        result.ToSuccess().Data!.AccessToken.Should().Be(accessToken.Value);
        result.ToSuccess().Data!.ExpiresIn.Should().Be(accessToken.MinutesExpiry);
    }
}
