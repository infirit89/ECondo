using System.Reflection;
using ECondo.Application.Services;
using ECondo.Application.Shared;
using ECondo.Domain.Authorization;
using ECondo.Domain.Exceptions;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using MediatR;

namespace ECondo.Application.Behaviours;

internal sealed class AuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IAuthorizationService authorizationService, IUserContext userContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IResourcePolicy
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var canPerformActionMethod = GetCanPerformActionMethod(request);
        var canPerformAction = (Task<bool>)canPerformActionMethod
            .Invoke(authorizationService, 
            [userContext.UserId, request.ResourceId,
            request.ResourceAction, cancellationToken])!;
        
        if (await canPerformAction)
            return await next();

        if(typeof(TResponse).IsResultType())
            return Utils.InvokeResultFail<TResponse>([CreateForbiddenError()]);

        throw new ForbiddenException();
    }

    private static Error CreateForbiddenError() =>
        Error.Forbidden("Resource.Forbidden", "The user does not have the needed permission");

    private static MethodInfo GetCanPerformActionMethod(TRequest request) =>
        typeof(IAuthorizationService)
            .GetMethod(nameof(IAuthorizationService.CanPerformActionAsync))!
            .MakeGenericMethod(request.ResourceType);
}