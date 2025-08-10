using ECondo.SharedKernel.Result;
using MediatR;

namespace ECondo.Application.Commands;

internal interface ICommandHandler<in TCommand, TResponse, TError>
    : IRequestHandler<TCommand, Result<TResponse, TError>>
    where TCommand : ICommand<TResponse, TError>;

internal interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse, Error>>
    where TCommand : ICommand<TResponse>;

internal interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Result<EmptySuccess, Error>>
    where TCommand : ICommand;