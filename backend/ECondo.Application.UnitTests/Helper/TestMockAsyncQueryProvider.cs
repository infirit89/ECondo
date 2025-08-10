using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ECondo.Application.UnitTests.Helper;

public class TestAsyncQueryProvider(IQueryProvider inner) : IAsyncQueryProvider
{
    public IQueryable CreateQuery(Expression expression)
    {
        var createQueryMethod = inner.GetType().GetMethod("CreateQuery", [typeof(Expression)]);
        return (IQueryable)createQueryMethod!.Invoke(inner, [expression])!;
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(inner.CreateQuery<TElement>(expression));
    }

    public object? Execute(Expression expression)
        => inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult);
        
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var taskResultType = resultType.GetGenericArguments()[0];
            var syncResult = inner.Execute(expression);
            
            // Create Task<T> from the result using reflection
            var taskFromResult = typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(taskResultType)
                .Invoke(null, [syncResult]);
                
            return (TResult)taskFromResult!;
        }
        
        if (resultType == typeof(Task))
        {
            return (TResult)(object)Task.CompletedTask;
        }

        return (TResult)inner.Execute(expression)!;
    }
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider(this);
}

public class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
{
    public T Current => inner.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return ValueTask.FromResult(inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        inner.Dispose();
        return ValueTask.CompletedTask;
    }
}