using ECondo.SharedKernel.Result;

namespace ECondo.SharedKernel.Extensions;

public static class TypeExtensions
{
    public static bool IsResultType(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(Result<,>);
}