using ECondo.Application.Commands.Identity.Login;
using ECondo.Application.Services;
using ECondo.Application.Extensions;
using ECondo.Application.Data;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity.Login;

public class LoginCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IdentityErrorDescriber _errorDescriber;
    private readonly IAuthTokenService _authTokenService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);
        _errorDescriber = new IdentityErrorDescriber();
        _authTokenService = Substitute.For<IAuthTokenService>();
        _handler = new LoginCommandHandler(_userManager, _errorDescriber, _authTokenService);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidUserError_WhenUserNotFound()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password123");
        _userManager.FindUserByEmailOrNameAsync(command.Email).Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.As<Result<TokenResult, Error>.Error>().Data
            .Should().BeEquivalentTo(UserErrors.InvalidUser(command.Email));
    }

    [Fact]
    public async Task Handle_ShouldReturnPasswordMismatchError_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var command = new LoginCommand(user.Email, "wrongpassword");
        _userManager.FindUserByEmailOrNameAsync(command.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, command.Password).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<ValidationError>();
        var validationError = (ValidationError)result.ToError().Data!;
        validationError.Errors.Should().ContainSingle(e => e.Code == "PasswordMismatch");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmailNotConfirmedError_WhenEmailIsNotConfirmed()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var command = new LoginCommand(user.Email, "password123");
        _userManager.FindUserByEmailOrNameAsync(command.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, command.Password).Returns(true);
        _userManager.IsEmailConfirmedAsync(user).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.As<Result<TokenResult, Error>.Error>().Data
            .Should().BeEquivalentTo(UserErrors.EmailNotConfirmed());
    }

    [Fact]
    public async Task Handle_ShouldReturnTokenResult_WhenLoginIsSuccessful()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var command = new LoginCommand(user.Email, "password123");
        var accessToken = new AccessToken { Value = "access-token", MinutesExpiry = 60 };
        var refreshToken = new RefreshToken { Value = "refresh-token", Expires = DateTime.UtcNow.AddDays(7) };

        _userManager.FindUserByEmailOrNameAsync(command.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, command.Password).Returns(true);
        _userManager.IsEmailConfirmedAsync(user).Returns(true);
        _authTokenService.GenerateAccessTokenAsync(user).Returns(accessToken);
        _authTokenService.GenerateRefreshTokenAsync(user).Returns(refreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        result.As<Result<TokenResult, Error>.Success>().Data
            .Should().BeEquivalentTo(new TokenResult(
                accessToken.Value,
                accessToken.MinutesExpiry,
                refreshToken.Value));
        await _authTokenService.Received(1).StoreRefreshTokenAsync(refreshToken);
    }
}
