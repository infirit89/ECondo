using ECondo.Application.Commands.Identity.ForgotPassword;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NSubstitute;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity.ForgotPassword;

public class ForgotPasswordCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly ForgotPasswordCommandHandler _handler;

    public ForgotPasswordCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);
        _emailService = Substitute.For<IEmailService>();
        _handler = new ForgotPasswordCommandHandler(_userManager, _emailService);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidUserError_WhenUserNotFound()
    {
        // Arrange
        var command = new ForgotPasswordCommand("nonexistent@example.com", "https://example.com/reset");
        _userManager.FindByEmailAsync(command.Username).Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldSendPasswordResetEmail_WhenUserExists()
    {
        // Arrange
        var command = new ForgotPasswordCommand("test@example.com", "https://example.com/reset");
        var user = new User { Email = command.Username };
        var token = "reset-token";

        _userManager.FindByEmailAsync(command.Username).Returns(user);
        _userManager.GeneratePasswordResetTokenAsync(user).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        await _emailService.Received(1).SendPasswordResetMail(
            user.Email!,
            Arg.Is<string>(url => url.Contains("token=reset-token") && url.Contains("email=test@example.com")));
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectQueryString_WhenSendingEmail()
    {
        // Arrange
        var command = new ForgotPasswordCommand("test@example.com", "https://example.com/reset");
        var user = new User { Email = command.Username };
        var token = "reset-token";

        _userManager.FindByEmailAsync(command.Username).Returns(user);
        _userManager.GeneratePasswordResetTokenAsync(user).Returns(token);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _emailService.Received(1).SendPasswordResetMail(
            user.Email!,
            Arg.Is<string>(url =>
                url == QueryHelpers.AddQueryString(command.ReturnUri, new Dictionary<string, string?>
                {
                    { "token", token },
                    { "email", command.Username }
                })));
    }
}

