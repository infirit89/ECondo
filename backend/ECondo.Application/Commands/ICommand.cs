using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands;

internal interface ICommand
    : ICommand<EmptySuccess>;

internal interface ICommand<TResponse>
    : ICommand<TResponse, Error>;

internal interface ICommand<TResponse, TError>
    : IRequest<Result<TResponse, TError>>;
