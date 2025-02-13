namespace ECondo.Domain.Shared;

public abstract record Result<TSuccess, TError>
{
    internal Result()
    {
    }

    public record Success(TSuccess? Data = default) : Result<TSuccess, TError>;

    public record Error(TError? Data = default) : Result<TSuccess, TError>;

    public static Result<TSuccess, TError> Ok(TSuccess? data = default) => new Success(data);
    public static Result<TSuccess, TError> Fail(TError? data = default) => new Error(data);
}

public class Empty;

public static class ResultHelper
{
    public static Result<TSuccess, TError>.Success ToSuccess<TSuccess, TError>(this Result<TSuccess, TError> result)
        => (Result<TSuccess, TError>.Success)result;

    public static Result<TSuccess, TError>.Error ToError<TSuccess, TError>(this Result<TSuccess, TError> result)
        => (Result<TSuccess, TError>.Error)result;
}
