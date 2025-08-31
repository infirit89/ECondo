using ECondo.Application.Commands.Identity.UpdatePassword;
using ECondo.Application.Services;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands.Identity
{
    public class UpdatePasswordCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly UpdatePasswordCommandHandler _handler;
        private readonly Guid _userId = Guid.NewGuid();

        public UpdatePasswordCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var userContext = Substitute.For<IUserContext>();
            userContext.UserId.Returns(_userId);
            _handler = new UpdatePasswordCommandHandler(_userManager, userContext);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var command = new UpdatePasswordCommand("currentPassword", "newPassword");
            _userManager.FindByIdAsync(_userId.ToString()).Returns((User)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.ToError().Data.Should().BeEquivalentTo(UserErrors.InvalidUser());
        }

        [Fact]
        public async Task Handle_ChangePasswordFails_ReturnsValidationError()
        {
            // Arrange
            var user = new User { Id = _userId, Email = "test@example.com" };
            var command = new UpdatePasswordCommand("currentPassword", "newPassword");
            var identityErrors = new IdentityError[]
            {
                new() { Code = "PasswordMismatch", Description = "Incorrect password." },
                new() { Code = "PasswordRequiresNonAlphanumeric", Description = "Passwords must have at least one non alphanumeric character." }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByIdAsync(_userId.ToString()).Returns(user);
            _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            var error = result.ToError().Data;
            error.Should().BeOfType<ValidationError>();
            var validationError = error as ValidationError;
            validationError!.Errors.Should().HaveCount(2);
            validationError.Errors.Should().Contain(e => e.Code == "PasswordMismatch");
            validationError.Errors.Should().Contain(e => e.Code == "PasswordRequiresNonAlphanumeric");
        }

        [Fact]
        public async Task Handle_ChangePasswordSucceeds_ReturnsSuccess()
        {
            // Arrange
            var user = new User { Id = _userId, Email = "test@example.com" };
            var command = new UpdatePasswordCommand("currentPassword", "newPassword");
            var identityResult = IdentityResult.Success;

            _userManager.FindByIdAsync(_userId.ToString()).Returns(user);
            _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
            await _userManager.Received(1).ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
        }

        [Fact]
        public async Task Handle_WithEmptyCurrentPassword_ShouldFail()
        {
            // Arrange
            var user = new User { Id = _userId, Email = "test@example.com" };
            var command = new UpdatePasswordCommand("", "newPassword");
            var identityErrors = new IdentityError[]
            {
                new() { Code = "PasswordMismatch", Description = "Incorrect password." }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByIdAsync(_userId.ToString()).Returns(user);
            _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            var error = result.ToError().Data;
            error.Should().BeOfType<ValidationError>();
        }

        [Fact]
        public async Task Handle_WithEmptyNewPassword_ShouldFail()
        {
            // Arrange
            var user = new User { Id = _userId, Email = "test@example.com" };
            var command = new UpdatePasswordCommand("currentPassword", "");
            var identityErrors = new IdentityError[]
            {
                new() { Code = "PasswordTooShort", Description = "Passwords must be at least 6 characters." }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByIdAsync(_userId.ToString()).Returns(user);
            _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            var error = result.ToError().Data;
            error.Should().BeOfType<ValidationError>();
            var validationError = error as ValidationError;
            validationError!.Errors.Should().HaveCount(1);
            validationError.Errors.Should().Contain(e => e.Code == "PasswordTooShort");
        }

        [Fact]
        public async Task Handle_SameOldAndNewPassword_ShouldFail()
        {
            // Arrange
            var user = new User { Id = _userId, Email = "test@example.com" };
            var command = new UpdatePasswordCommand("samePassword", "samePassword");
            var identityErrors = new IdentityError[]
            {
                new() { Code = "PasswordRequiresUniqueChars", Description = "The new password must be different from the current password." }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByIdAsync(_userId.ToString()).Returns(user);
            _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            var error = result.ToError().Data;
            error.Should().BeOfType<ValidationError>();
            var validationError = error as ValidationError;
            validationError!.Errors.Should().HaveCount(1);
            validationError.Errors.Should().Contain(e => e.Code == "PasswordRequiresUniqueChars");
        }
    }
}