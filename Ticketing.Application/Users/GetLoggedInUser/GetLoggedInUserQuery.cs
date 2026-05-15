using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;