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
            var command = new ResetPasswordCommand("test@example.com", "token", "newPassword");
            _userManager.FindByEmailAsync(command.Email).Returns((User)null);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<EmptySuccess, IdentityError[]>.Error>();
            result.As<Result<EmptySuccess, IdentityError[]>.Error>().Data.Should().ContainSingle(e => e.Code == "Users.NotFound");
        }

        [Fact]
        public async Task Handle_PasswordResetFails_ReturnsErrors()
        {
            var user = new User { Email = "test@example.com" };
            var command = new ResetPasswordCommand("test@example.com", "token", "newPassword");
            var identityErrors = new IdentityError[] { new IdentityError { Code = "InvalidToken", Description = "Invalid token" } };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword).Returns(identityResult);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<EmptySuccess, IdentityError[]>.Error>();
            result.As<Result<EmptySuccess, IdentityError[]>.Error>().Data.Should().ContainSingle(e => e.Code == "InvalidToken");
        }

        [Fact]
        public async Task Handle_PasswordResetSucceeds_ReturnsSuccess()
        {
            var user = new User { Email = "test@example.com" };
            var command = new ResetPasswordCommand("test@example.com", "token", "newPassword");
            var identityResult = IdentityResult.Success;

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword).Returns(identityResult);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<EmptySuccess, IdentityError[]>.Success>();
        }
    }
}
