using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;

public sealed record GenerateAccessTokenCommand(string RefreshToken) : IRequest<Result<TokenResult, Error>>;
