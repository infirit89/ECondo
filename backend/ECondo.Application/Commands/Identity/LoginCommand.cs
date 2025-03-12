using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<TokenResult, Error>>;
