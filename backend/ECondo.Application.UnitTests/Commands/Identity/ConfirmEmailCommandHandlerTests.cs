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
    public class ConfirmEmailCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly ConfirmEmailCommandHandler _handler;

        public ConfirmEmailCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _handler = new ConfirmEmailCommandHandler(_userManager);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var command = new ConfirmEmailCommand("token", "test@example.com");
            _userManager.FindByEmailAsync(command.Email).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error[]>.Error>();
            result.As<Result<EmptySuccess, Error[]>.Error>().Data.Should().ContainSingle(e => e.Code == "Users.NotFound");
        }

        [Fact]
        public async Task Handle_EmailConfirmationFails_ReturnsErrors()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ConfirmEmailCommand("token", "test@example.com");
            var identityErrors = new IdentityError[] { new IdentityError { Code = "InvalidToken", Description = "Invalid token" } };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ConfirmEmailAsync(user, command.Token).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error[]>.Error>();
            result.As<Result<EmptySuccess, Error[]>.Error>().Data.Should().ContainSingle(e => e.Code == "InvalidToken");
        }

        [Fact]
        public async Task Handle_EmailConfirmationSucceeds_ReturnsSuccess()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ConfirmEmailCommand("token", "test@example.com");
            var identityResult = IdentityResult.Success;

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.ConfirmEmailAsync(user, command.Token).Returns(identityResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error[]>.Success>();
        }
    }
}
