using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;
public sealed record InvalidateRefreshTokenCommand(string RefreshToken) : IRequest<Result<EmptySuccess, Error>>;