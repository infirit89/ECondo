using ECondo.Application.Queries.Profile;
using ECondo.Application.Services;
using ECondo.Application.Repositories;
using ECondo.Application.Data;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace ECondo.Application.UnitTests.Queries.Profile
{
    public class GetProfileQueryHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProfileDetails> _profileDetailsRepository;
        private readonly GetProfileQueryHandler _handler;

        public GetProfileQueryHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _profileDetailsRepository = Substitute.For<IRepository<ProfileDetails>>();
            _unitOfWork.ProfileDetails.Returns(_profileDetailsRepository);
            _handler = new GetProfileQueryHandler(_userManager, _unitOfWork);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            var query = new GetProfileQuery("test@example.com");
            _userManager.FindByEmailAsync(query.Email).Returns((User)null);

            
            var result = await _handler.Handle(query, CancellationToken.None);

            
            result.Should().BeOfType<Result<ProfileResult, Error>.Error>();
            result.As<Result<ProfileResult, Error>.Error>().Data.Code.Should().Be("Users.NotFound");
        }

        [Fact]
        public async Task Handle_ProfileNotFound_ReturnsInvalidProfileError()
        {
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
            var query = new GetProfileQuery("test@example.com");

            _userManager.FindByEmailAsync(query.Email).Returns(user);
            _profileDetailsRepository.GetAsync(pd => pd.UserId == user.Id).Returns(Enumerable.Empty<ProfileDetails>());

            
            var result = await _handler.Handle(query, CancellationToken.None);

            
            result.Should().BeOfType<Result<ProfileResult, Error>.Error>();
            result.As<Result<ProfileResult, Error>.Error>().Data.Code.Should().Be("Profile.NotFound");
        }

        [Fact]
        public async Task Handle_ValidUserAndProfile_ProfileRetrievedSuccessfully()
        {
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
            var profile = new ProfileDetails
            {
                UserId = user.Id,
                FirstName = "FirstName",
                MiddleName = "MiddleName",
                LastName = "LastName"
            };
            var query = new GetProfileQuery("test@example.com");

            _userManager.FindByEmailAsync(query.Email).Returns(user);
            _profileDetailsRepository.GetAsync(pd => pd.UserId == user.Id).Returns(new[] { profile });
            
            
            var result = await _handler.Handle(query, CancellationToken.None);

            
            result.Should().BeOfType<Result<ProfileResult, Error>.Success>();
            result.ToSuccess().Data?.FirstName.Should().Be("FirstName");
            result.ToSuccess().Data?.MiddleName.Should().Be("MiddleName");
            result.ToSuccess().Data?.LastName.Should().Be("LastName");
        }
    }
}
