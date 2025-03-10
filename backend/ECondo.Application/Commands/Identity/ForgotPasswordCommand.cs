using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;

public sealed record ForgotPasswordCommand(string Username, string ReturnUri) : IRequest<Result<EmptySuccess, Error>>;
