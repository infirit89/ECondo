using ECondo.SharedKernel.Result;

namespace ECondo.Api.Extensions;

internal static class ResultExtensions
{
    public static TOut Match<TIn, TError, TOut>(
        this Result<TIn, TError> result,
        Func<TIn, TOut> onSuccess,
        Func<TError, TOut> onFailure)
    {
        return result.IsOk() ? onSuccess(result.ToSuccess().Data!) : onFailure(result.ToError().Data!);
    }
}