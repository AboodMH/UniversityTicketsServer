using Ticketing.Application.Abstractions.Data;
using Dapper;

namespace Ticketing.Api.Extensions;

public static class SeedDataExtensions
{
    public static async Task SeedUsersAsync(this IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using var connection = sqlConnectionFactory.CreateConnection();

            var httpClient = new HttpClient();

            var usersToSeed = new[]
            {
                new { FirstName = "First", LastName = "Student", Email = "student@gmail.com", Password = "123456", Role = "Student" },
                new { FirstName = "First", LastName = "Instructor", Email = "instructor@gmail.com", Password = "123456", Role = "Instructor" }
            };

            var tokenResponse = await httpClient.PostAsync(
                "http://ticketing-idp:8080/realms/ticketing/protocol/openid-connect/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", "ticketing-admin-client" },
                    { "client_secret", "JZzEJmQgUm57ocqnrOkJBrvPOTsWrKrh" }
                }));

            if (!tokenResponse.IsSuccessStatusCode)
            {
                var errorContent = await tokenResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"[Seed Error] Failed to get admin token. Status: {tokenResponse.StatusCode}, Details: {errorContent}");
                return;
            }

            var tokenData = await tokenResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            var accessToken = tokenData?["access_token"]?.ToString();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            const string sql = """
                INSERT INTO public.users (id, first_name, last_name, email, identity_id, role)
                VALUES (@Id, @FirstName, @LastName, @Email, @IdentityId, @Role)
                ON CONFLICT (email) DO NOTHING;
                """;

            foreach (var user in usersToSeed)
            {
                var keycloakUser = new
                {
                    username = user.Email,
                    email = user.Email,
                    enabled = true,
                    emailVerified = true,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    credentials = new[]
                    {
                        new { type = "password", value = user.Password, temporary = false }
                    }
                };

                var createResponse = await httpClient.PostAsJsonAsync(
                    "http://ticketing-idp:8080/admin/realms/ticketing/users",
                    keycloakUser);

                if (!createResponse.IsSuccessStatusCode && createResponse.StatusCode != System.Net.HttpStatusCode.Conflict)
                {
                    var userError = await createResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Seed Error] Failed to create user {user.Email} in Keycloak. Status: {createResponse.StatusCode}, Details: {userError}");
                    continue;
                }

                string identityId = Guid.NewGuid().ToString();
                if (createResponse.Headers.Location != null)
                {
                    identityId = createResponse.Headers.Location.Segments.Last();
                }

                await connection.ExecuteAsync(sql, new
                {
                    Id = Guid.NewGuid(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IdentityId = identityId,
                    Role = user.Role
                });

                Console.WriteLine($"[Seed Success] User {user.Email} added successfully to Keycloak and DB.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Seed Exception] Something went wrong: {ex.Message}");
        }
    }
}