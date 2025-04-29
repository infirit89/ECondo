//using ECondo.Application.Repositories;
//using ECondo.Application.Services;
//using ECondo.Domain.Profiles;
//using ECondo.Domain.Shared;
//using ECondo.Domain.Users;
//using FluentAssertions;
//using Microsoft.AspNetCore.Identity;
//using NSubstitute;
//using System.Threading;
//using System.Threading.Tasks;
//using ECondo.Application.Commands.Profiles.Create;
//using Xunit;

//namespace ECondo.Application.UnitTests.Commands.Profile
//{
//    public class CreateProfileCommandHandlerTests
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IRepository<ProfileDetails> _profileDetailsRepository;
//        private readonly CreateProfileCommandHandler _handler;

//        public CreateProfileCommandHandlerTests()
//        {
//            _userManager = Substitute.For<UserManager<User>>(
//                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
//            _unitOfWork = Substitute.For<IUnitOfWork>();
//            _profileDetailsRepository = Substitute.For<IRepository<ProfileDetails>>();
//            _unitOfWork.ProfileDetails.Returns(_profileDetailsRepository);
//            _handler = new CreateProfileCommandHandler(_userManager, _unitOfWork);
//        }

//        [Fact]
//        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
//        {
//            // Arrange
//            var command = new CreateProfileCommand("test@example.com", "FirstName", "MiddleName", "LastName", "0881231231");
//            _userManager.FindByEmailAsync(command.Username).Returns((User)null);
//            _userManager.FindByNameAsync(command.Username).Returns((User)null);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
//            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Users.NotFound");
//        }

//        [Fact]
//        public async Task Handle_ValidUser_CreatesProfileSuccessfully()
//        {
//            // Arrange
//            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
//            var command = new CreateProfileCommand("test@example.com", "FirstName", "MiddleName", "LastName", "0881231231");

//            _userManager.FindByEmailAsync(command.Username).Returns(user);
//            _userManager.FindByNameAsync(command.Username).Returns(user);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            await _profileDetailsRepository.Received(1).InsertAsync(Arg.Is<ProfileDetails>(pd =>
//                pd.FirstName == command.FirstName &&
//                pd.MiddleName == command.MiddleName &&
//                pd.LastName == command.LastName &&
//                pd.UserId == user.Id &&
//                pd.User == user));
//            await _unitOfWork.Received(1).SaveChangesAsync();
//            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
//        }
//    }
//}
