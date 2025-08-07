using ECondo.Application.Behaviours;
using ECondo.Application.Services;
using ECondo.Domain.Authorization;
using ECondo.Domain.Exceptions;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using MediatR;
using NSubstitute;

namespace ECondo.Application.UnitTests.Authorization;

public class AuthorizationPipelineBehaviourTests
{
    private readonly IAuthorizationService _mockAuthorizationService;
    private readonly IUserContext _mockUserContext;
    private readonly AuthorizationPipelineBehaviour<TestCommand, Result<string, Error>> _behaviour;
    private readonly RequestHandlerDelegate<Result<string, Error>> _mockNext;

    public AuthorizationPipelineBehaviourTests()
    {
        _mockAuthorizationService = Substitute.For<IAuthorizationService>();
        _mockUserContext = Substitute.For<IUserContext>();
        _behaviour = new AuthorizationPipelineBehaviour<TestCommand, Result<string, Error>>(_mockAuthorizationService, _mockUserContext);
        _mockNext = Substitute.For<RequestHandlerDelegate<Result<string, Error>>>();
    }

    [Fact]
    public async Task Handle_UserHasPermission_CallsNextHandler()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new TestCommand { ResourceId = Guid.NewGuid() };
        var expectedResult = Result<string, Error>.Ok("test");

        _mockUserContext.UserId.Returns(userId);
        _mockAuthorizationService
            .CanPerformActionAsync<TestEntity>(userId, request.ResourceId, request.ResourceAction, Arg.Any<CancellationToken>())
            .Returns(true);
        _mockNext().Returns(expectedResult);

        // Act
        var result = await _behaviour.Handle(request, _mockNext, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        await _mockNext.Received(1)();
    }

    [Fact]
    public async Task Handle_UserDoesNotHavePermission_ReturnsFailureResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new TestCommand { ResourceId = Guid.NewGuid() };

        _mockUserContext.UserId.Returns(userId);
        _mockAuthorizationService
            .CanPerformActionAsync<TestEntity>(userId, request.ResourceId, request.ResourceAction, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _behaviour.Handle(request, _mockNext, CancellationToken.None);

        // Assert
        Assert.True(!result.IsOk());
        Assert.Equal("Resource.Forbidden", result.ToError().Data.Code);
        await _mockNext.DidNotReceive()();
    }

    [Fact]
    public async Task Handle_UserDoesNotHavePermission_NonResultType_ThrowsForbiddenException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new TestCommand { ResourceId = Guid.NewGuid() };
        var nonResultBehaviour = new AuthorizationPipelineBehaviour<TestCommand, string>(_mockAuthorizationService, _mockUserContext);
        var mockNextString = Substitute.For<RequestHandlerDelegate<string>>();

        _mockUserContext.UserId.Returns(userId);
        _mockAuthorizationService
            .CanPerformActionAsync<TestEntity>(userId, request.ResourceId, request.ResourceAction, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => 
            nonResultBehaviour.Handle(request, mockNextString, CancellationToken.None));

        await mockNextString.DidNotReceive()();
    }

    private class TestCommand : IResourcePolicy<TestEntity>
    {
        public Guid? ResourceId { get; set; }
        public Type ResourceType => typeof(TestEntity);
        public AccessLevel ResourceAction => AccessLevel.Read;
    }

    private class TestEntity
    {
    }
}