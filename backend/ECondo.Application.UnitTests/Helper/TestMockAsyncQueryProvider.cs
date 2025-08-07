using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ECondo.Application.UnitTests.Helper;

public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider 
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
        => new TestQueryable<TEntity>(_inner.CreateQuery(expression).Cast<TEntity>());

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new TestQueryable<TElement>(_inner.CreateQuery<TElement>(expression));

    public object? Execute(Expression expression)
        => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => _inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult);
        
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var taskResultType = resultType.GetGenericArguments()[0];
            var syncResult = _inner.Execute(expression);
            
            // Create Task<T> from the result using reflection
            var taskFromResult = typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(taskResultType)
                .Invoke(null, new[] { syncResult });
                
            return (TResult)taskFromResult!;
        }
        
        if (resultType == typeof(Task))
        {
            return (TResult)(object)Task.CompletedTask;
        }

        return (TResult)_inner.Execute(expression)!;
    }
}