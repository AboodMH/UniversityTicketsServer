using Ticketing.Domain.Users;

namespace Ticketing.Api.Controllers.Users;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    Role Role);