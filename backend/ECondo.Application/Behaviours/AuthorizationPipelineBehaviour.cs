using ECondo.Application.Shared;
using MediatR;

namespace ECondo.Application.Behaviours;

internal sealed class AuthorizationPipelineBehaviour
    <TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthRequirement
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // the user has the auth requirement, succeed else fail
        throw new NotImplementedException();
    }
}