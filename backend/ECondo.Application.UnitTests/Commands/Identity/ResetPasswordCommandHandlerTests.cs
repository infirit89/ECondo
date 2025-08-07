using ECondo.Application.Commands.Identity.ResetPassword;
using ECondo.Application.Extensions;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity
{
    public class ResetPasswordCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly ResetPasswordCommandHandler _handler;

        public ResetPasswordCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _handler = new ResetPasswordCommandHandler(_userManager);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var command = new ResetPasswordCommand("test@example.com", "token", "newPassword");
            _userManager.FindByEmailAsync(command.Email).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.ToError().Data.Should().BeEquivalentTo(UserErrors.InvalidUser(command.Email));
        }

        [Fact]
        public async Task Handle_PasswordResetFails_ReturnsValidationError()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ResetPasswordCommand("test@example.com", "token", "newPassword");
            var identityErrors = new IdentityError[]
            {
                new() { Code = "InvalidToken", Description = "Invalid token" },
                new() { Code = "PasswordRequiresDigit", Description = "Passwords must have at least one digit" }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            var error = result.ToError().Data;
            error.Should().BeOfType<ValidationError>();
            var validationError = (ValidationError)error;
            validationError.Errors.Should().HaveCount(2);
            validationError.Errors.Should().Contain(e => e.Code == "InvalidToken");
            validationError.Errors.Should().Contain(e => e.Code == "PasswordRequiresDigit");
        }

        [Fact]
        public async Task Handle_PasswordResetSucceeds_ReturnsSuccess()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ResetPasswordCommand("test@example.com", "token", "newPassword");
            var identityResult = IdentityResult.Success;

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
            await _userManager.Received(1).ResetPasswordAsync(user, command.Token, command.NewPassword);
        }

        [Fact]
        public async Task Handle_WithEmptyToken_ShouldStillProcessRequest()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ResetPasswordCommand("test@example.com", "", "newPassword");
            var identityResult = IdentityResult.Success;

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
            await _userManager.Received(1).ResetPasswordAsync(user, "", command.NewPassword);
        }

        [Fact]
        public async Task Handle_WithEmptyPassword_ShouldFailValidation()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ResetPasswordCommand("test@example.com", "token", "");
            var identityErrors = new IdentityError[]
            {
                new() { Code = "PasswordTooShort", Description = "Password is too short" }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            var error = result.ToError().Data;
            error.Should().BeOfType<ValidationError>();
            var validationError = (ValidationError)error;
            validationError.Errors.Should().HaveCount(1);
            validationError.Errors.Should().Contain(e => e.Code == "PasswordTooShort");
        }
    }
}