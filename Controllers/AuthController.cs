using System.Security.Claims;
using GameApi2.DTOs;
using GameApi2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameApi2.Controllers;

public static class AuthController
{
    public static async Task MapAuthEndpoints(this WebApplication app)

    {

#pragma warning disable ASPDEPR002 // Type or member is obsolete
        app.MapPost("/auth/register", async ([FromBody] RegisterDto? dto, AuthService auth) =>
        {
            if (dto == null)
                return Results.BadRequest(new { error = "Request body mangler." });

            var result = await auth.RegisterAsync(dto);
            if (result == null)
                return Results.BadRequest(new { error = "Email findes allerede." });

            // Returnér token + user (som login), så klienten er logget ind med det samme
            return Results.Created("/auth/login", result);
        })
        .WithName("Register")
        .WithOpenApi();


        app.MapPost("/auth/login", async ([FromBody] LoginDto? dto, AuthService auth) =>
        {
            if (dto == null)
                return Results.BadRequest(new { error = "Request body mangler." });

            var result = await auth.LoginAsync(dto);

            if (result == null)
                return Results.Unauthorized();
            return Results.Ok(result);
        })
        .WithName("Login")
        .WithOpenApi();


        app.MapGet("/auth/users", async (UserService userService) =>
        {

            var users = await userService.GetAllUsersAsync();
            return Results.Ok(users);

        }).RequireAuthorization("AdminOnly"); // kræver policy "AdminOnly" (RequireRole("Admin"))


        app.MapGet("/auth/users/{id}", async (string id, ClaimsPrincipal principal, UserService userService) =>
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = principal.IsInRole("Admin");
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();
            if (!isAdmin && userId != id)
                return Results.Forbid();
            var user = await userService.GetUserByIdAsync(id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        })
        .RequireAuthorization()
        .WithOpenApi();

    }


}