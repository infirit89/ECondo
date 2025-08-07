using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using MediatR;

namespace ECondo.Application.Queries;

internal interface IQuery
    : IQuery<EmptySuccess>;

internal interface IQuery<TResponse>
    : IQuery<TResponse, Error>;

internal interface IQuery<TResponse, TError> 
    : IRequest<Result<TResponse, TError>>;
