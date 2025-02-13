using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;

public sealed record GenerateAccessTokenCommand(string RefreshToken) : IRequest<Result<TokenResult, IdentityError>>;
