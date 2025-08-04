using ECondo.Domain.Shared;

namespace ECondo.Application.Shared;

internal static class Utils
{
    public static bool IsResultType(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(Result<,>);
    
    public static bool IsTypeResultType<T>()
        => typeof(T).IsGenericType &&
           typeof(T).GetGenericTypeDefinition() == typeof(Result<,>);

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