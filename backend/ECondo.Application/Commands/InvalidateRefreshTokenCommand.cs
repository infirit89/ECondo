using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;
public sealed record InvalidateRefreshTokenCommand(string Username, string RefreshToken) : IRequest<Result<Empty, IdentityError>>;