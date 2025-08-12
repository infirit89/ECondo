namespace ECondo.SharedKernel.Result;

public abstract record Result<TSuccess, TError>
{
    internal Result()
    {
    }

    public record Success(TSuccess? Data = default) : Result<TSuccess, TError>;

    public record Error(TError? Data = default) : Result<TSuccess, TError>;

    public static Result<TSuccess, TError> Ok(TSuccess? data = default) => new Success(data);
    public static Result<TSuccess, TError> Fail(TError? data = default) => new Error(data);

    public bool IsOk() => this is Success;
}

public class EmptySuccess;
public class EmptyError;

public static class ResultHelper
{
    public static Result<TSuccess, TError>.Success ToSuccess<TSuccess, TError>(this Result<TSuccess, TError> result)
        => (Result<TSuccess, TError>.Success)result;

    public static Result<TSuccess, TError>.Error ToError<TSuccess, TError>(this Result<TSuccess, TError> result)
        => (Result<TSuccess, TError>.Error)result;
    
    public static T InvokeResultFail<T>(object?[]? parameters)
    {
        Type resultType = typeof(T).GetGenericArguments()[0];
        var failMethodInfo = typeof(Result<,>)
            .MakeGenericType(resultType, typeof(Error))
            .GetMethod(nameof(Result<object, Error>.Fail));

        object? res = failMethodInfo?.Invoke(
            null,
            parameters);

        if (res is not null)
            return (T)res;

        return default!;
    }
}
