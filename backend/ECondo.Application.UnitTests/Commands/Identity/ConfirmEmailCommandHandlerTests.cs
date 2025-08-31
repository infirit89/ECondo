using ECondo.Application.Commands.Identity.ConfirmEmail;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands.Identity.ConfirmEmail;

public class ConfirmEmailCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly ConfirmEmailCommandHandler _handler;

    public ConfirmEmailCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);
        _handler = new ConfirmEmailCommandHandler(_userManager);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidUserError_WhenUserNotFound()
    {
        // Arrange
        var command = new ConfirmEmailCommand("nonexistent@example.com", "token");
        _userManager.FindByEmailAsync(command.Email).Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationErrors_WhenConfirmEmailFails()
    {
        // Arrange
        var command = new ConfirmEmailCommand("test@example.com", "invalid-token");
        var user = new User { Email = command.Email };
        _userManager.FindByEmailAsync(command.Email).Returns(user);
        _userManager.ConfirmEmailAsync(user, command.Token)
            .Returns(IdentityResult.Failed(new IdentityError { Code = "Error", Description = "Confirmation failed" }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<ValidationError>();
        var validationError = (ValidationError)result.ToError().Data!;
        validationError.Errors.Should().ContainSingle(e => e.Code == "Error");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenEmailConfirmationIsSuccessful()
    {
        // Arrange
        var command = new ConfirmEmailCommand("test@example.com", "valid-token");
        var user = new User { Email = command.Email };
        _userManager.FindByEmailAsync(command.Email).Returns(user);
        _userManager.ConfirmEmailAsync(user, command.Token).Returns(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
    }
}

