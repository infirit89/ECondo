using ECondo.Application.Commands.Identity;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity
{
    public class UpdatePasswordCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly UpdatePasswordCommandHandler _handler;

        public UpdatePasswordCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _handler = new UpdatePasswordCommandHandler(_userManager);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            var command = new UpdatePasswordCommand("test@example.com", "oldPassword", "newPassword");
            _userManager.FindByEmailAsync(command.Email).Returns((User)null);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<EmptySuccess, Error[]>.Error>();
            result.As<Result<EmptySuccess, Error[]>.Error>().Data.Should().ContainSingle(e => e.Code == "Users.NotFound");
        }

        [Fact]
        public async Task Handle_PasswordChangeFails_ReturnsErrors()
        {
            var user = new User { Email = "test@example.com" };
            var command = new UpdatePasswordCommand("test@example.com", "oldPassword", "newPassword");
            var identityErrors = new IdentityError[] { new IdentityError { Code = "PasswordMismatch", Description = "Password mismatch" } };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword).Returns(identityResult);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<EmptySuccess, Error[]>.Error>();
            result.As<Result<EmptySuccess, Error[]>.Error>().Data.Should().ContainSingle(e => e.Code == "PasswordMismatch");
        }

        [Fact]
        public async Task Handle_PasswordChangeSucceeds_ReturnsSuccess()
        {
            var user = new User { Email = "test@example.com" };
            var command = new UpdatePasswordCommand("test@example.com", "oldPassword", "newPassword");
            var identityResult = IdentityResult.Success;

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword).Returns(identityResult);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<EmptySuccess, Error[]>.Success>();
        }
    }
}
