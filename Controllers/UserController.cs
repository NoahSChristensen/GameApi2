using GameApi2.DTOs;
using GameApi2.Models;
using GameApi2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameApi2.Controllers;

public static class UserController
{

    public static async Task MapUserEndpoints(this WebApplication app)
    {

        app.MapGet("/users", async (UserService userService) =>
        {
            var users = await userService.GetAllUsersAsync();
            return Results.Ok(users);
        }).WithName("GetAllUsers");

        app.MapGet("/users/{id}", async (string id, UserService userService) =>
        {
            var user = await userService.GetUserByIdAsync(id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }).WithName("GetUserById");

        app.MapGet("/user/email/{email}", async (string email, UserService userService) =>
        {
            var user = await userService.GetUserByEmailAsync(email);
            return user != null ? Results.Ok(user) : Results.NotFound();
        });

        app.MapPost("/users", async ([FromBody] UserPostDto postDto, UserService userService) =>
        {
            var user = await userService.CreateUserAsync(postDto);
            return Results.Created($"/users/{user.Id}", user);
        }).WithName("CreatedUser");

        app.MapPatch("/users{id}", async (string id, [FromBody] UserPatchDto patchDto, UserService userService) =>
        {
            var user = await userService.PatchUserAsync(id, patchDto);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }).WithName("PatchUser");


        app.MapPut("/users/{id}", async (string id, [FromBody] UserUpdateDto updateDto, UserService userService) =>
        {
            var user = await userService.UpdateUserAsync(id, updateDto);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }).WithName("UpdateUser");


        app.MapDelete("/users/{id}", async (string id, [FromBody] UserDeleteDto? deleteDto, UserService userService) =>
        {
            var deleted = await userService.DeleteUserAsync(id, deleteDto);
            return deleted ? Results.NoContent() : Results.NotFound();
        }).WithName("DeleteUser");

    }

}