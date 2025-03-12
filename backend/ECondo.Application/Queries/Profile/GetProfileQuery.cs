using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Queries.Profile;

public sealed record GetProfileQuery(string Email) : IRequest<Result<ProfileResult, Error>>;
