using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Events.Identity;
public sealed record UserRegisteredEvent(User User, string ReturnUri) : INotification;