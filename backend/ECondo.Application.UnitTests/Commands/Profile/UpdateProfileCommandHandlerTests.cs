using ECondo.Application.Commands.Profile;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Profile
{
    public class UpdateProfileCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProfileDetails> _profileDetailsRepository;
        private readonly UpdateProfileCommandHandler _handler;

        public UpdateProfileCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _profileDetailsRepository = Substitute.For<IRepository<ProfileDetails>>();
            _unitOfWork.ProfileDetailsRepository.Returns(_profileDetailsRepository);
            _handler = new UpdateProfileCommandHandler(_userManager, _unitOfWork);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var command = new UpdateProfileCommand("test@example.com", "FirstName", "MiddleName", "LastName");
            _userManager.FindByEmailAsync(command.Email).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Users.NotFound");
        }

        [Fact]
        public async Task Handle_ProfileNotFound_ReturnsInvalidProfileError()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
            var command = new UpdateProfileCommand("test@example.com", "FirstName", "MiddleName", "LastName");

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _profileDetailsRepository.GetAsync(x => x.UserId == user.Id).Returns(Enumerable.Empty<ProfileDetails>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Profile.NotFound");
        }

        [Fact]
        public async Task Handle_ValidUserAndProfile_ProfileUpdatedSuccessfully()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
            var profile = new ProfileDetails { UserId = user.Id };
            var command = new UpdateProfileCommand("test@example.com", "FirstName", "MiddleName", "LastName");

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _unitOfWork.ProfileDetailsRepository.GetAsync(x => x.UserId == user.Id).Returns(new[] { profile });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _profileDetailsRepository.Received(1).Update(profile);
            await _unitOfWork.Received(1).SaveChangesAsync();
            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
        }
    }
}

