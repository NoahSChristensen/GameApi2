using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GameApi2.Data;
using GameApi2.Models;
using System.Security.Claims;
using GameApi2.Services;
using GameApi2.DTOs;
using Sprache;

namespace GameApi2.Controllers;

public static class MazeController
{
    public static async Task MapMazeEndPoints(this WebApplication app)
    {

        app.MapPost("/maze/unlock/{levelId}", async (int levelId, [FromServices] MazeService mazeService, ClaimsPrincipal principal) =>
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            try
            {
                await mazeService.UnlockNextLevelAsync(userId, levelId);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500, title: "Der opstod en fejl ved unlock af maze");
            }

        }).RequireAuthorization().WithName("UnlockNextMaze").WithTags("Maze");

        app.MapGet("/maze/unlocked", async (
            [FromServices] MazeService mazeService,
            ClaimsPrincipal principal) =>
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            var mazes = await mazeService.GetMazesAsync(userId);
            return Results.Ok(mazes);

        }).RequireAuthorization().WithName("GetUnlockedMazes").WithTags("Maze");
    }
}