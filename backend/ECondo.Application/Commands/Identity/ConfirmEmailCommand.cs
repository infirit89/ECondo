using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;

public sealed record ConfirmEmailCommand(string Token, string Email) : IRequest<Result<EmptySuccess, Error[]>>;
