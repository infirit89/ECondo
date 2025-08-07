using ECondo.Application.Authorization;
using ECondo.Domain.Authorization;

namespace ECondo.Application.UnitTests.Authorization;

public class DefaultAuthorizationHandlerTests
{
    private readonly DefaultAuthorizationHandler<TestEntity> _handler;

    public DefaultAuthorizationHandlerTests()
    {
        _handler = new DefaultAuthorizationHandler<TestEntity>();
    }

    [Fact]
    public async Task GetAccessLevelAsync_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var resourceId = Guid.NewGuid();

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, resourceId);

        // Assert
        Assert.Equal(AccessLevel.None, result);
    }

    [Fact]
    public async Task GetAccessLevelAsync_WithNullResourceId_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, null);

        // Assert
        Assert.Equal(AccessLevel.None, result);
    }

    [Fact]
    public async Task ApplyDataFilterAsync_ReturnsUnmodifiedQuery()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = Guid.NewGuid() },
            new TestEntity { Id = Guid.NewGuid() },
            new TestEntity { Id = Guid.NewGuid() }
        }.AsQueryable();

        // Act
        var result = await _handler.ApplyDataFilterAsync(entities, userId);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Equal(entities, result);
    }

    private class TestEntity
    {
        public Guid Id { get; set; }
    }
}