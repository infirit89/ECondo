using ECondo.SharedKernel.Result;
using MediatR;

namespace ECondo.Application.Queries;

internal interface IQueryHandler<in TQuery, TResponse, TError>
    : IRequestHandler<TQuery, Result<TResponse, TError>>
    where TQuery : IQuery<TResponse, TError>;

internal interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse, Error>>
    where TQuery : IQuery<TResponse>;

internal interface IQueryHandler<in TQuery>
    : IRequestHandler<TQuery, Result<EmptySuccess, Error>>
    where TQuery : IQuery;