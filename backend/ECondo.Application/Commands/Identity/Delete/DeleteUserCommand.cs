using ECondo.Domain.Authorization;
using ECondo.Domain.Users;

namespace ECondo.Application.Commands.Identity.Delete;

public record DeleteUserCommand(Guid UserId) : ICommand, ICanDelete<User>
{
    Guid? IResourcePolicy.ResourceId => UserId;
}