using System.Drawing;
using System.Security.Claims;
using GameApi2.DTOs;
using GameApi2.Repositories;
using GameApi2.Services;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using SQLitePCL;


namespace GameApi2.Controllers;


public static class PointsController
{

    public static void MapEndPoints(this WebApplication app)
    {
        app.MapGet("/user/points", async (ClaimsPrincipal principal, PointService pointService) =>
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            try
            {
                var dto = pointService.GetPointAsync(userId);
                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500, title: "Der opstod en fejl ved hentning af point");
            }

        }).RequireAuthorization()
        .WithName("GetMyPoints")
        .WithTags("Points");


        app.MapPost("/user/points", async ([FromBody] PostPointRequest? dto, ClaimsPrincipal principal, PointService pointService) =>
        {

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();
            if (dto == null)
                return Results.BadRequest(new { error = "Request body mangler" });
            if (string.IsNullOrWhiteSpace(dto.Source))
                return Results.BadRequest(new { error = "Source er påkrævet" });
            if (dto.Amount < 1 || dto.Amount > 100)
                return Results.BadRequest(new { error = "Amount: mellem 1-100." });

            try
            {
                var result = await pointService.PostPointAsync(userId, dto.Source.Trim(), dto.Amount);

                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500, title: "Der opstod en fejl ved tilføjelse af point");
            }

        }).RequireAuthorization()
        .WithName("AddPoints")
        .WithTags("Points");


        app.MapGet("/users/points", async (PointService pointService) =>
        {

            try
            {
                var list = await pointService.GetAllPointsAsync();

                return Results.Ok(list);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500,
                    title: "Der opstod en fejl ved hentning af highscore");
            }

        }).RequireAuthorization()
        .WithName("GetAllPoints")
        .WithTags("Points");


        app.MapGet("/users/{id}/points", async (string id, PointService pointService) =>
        {

            try
            {
                var dto = await pointService.GetPointsForUserAsync(id);

                if (dto == null) return Results.NotFound(new { error = $"Bruger med Id {id} blev ikke fundet" });

                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500, title: "Der opstod en fejl ved hentning af point");
            }

        }).RequireAuthorization()
        .WithName("GetPointById")
        .WithTags("Points");


        app.MapPut("/users/{id}/points", async (string id, [FromBody] PutPointDto? dto, PointService pointService) =>
        {

            if (dto == null) return Results.BadRequest(new { error = "Request body mangler" });

            try
            {
                var result = await pointService.PutPointAsync(id,
                dto.Total);
                if (result == null)
                    return Results.NotFound(new { error = $"Bruger med ID {id} blev ikke fundet" });
                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500, title: "Der opstod fejl ved opdatering af point");
            }


        }).RequireAuthorization()
        .WithName("SetPoints")
        .WithTags("Points");
    }


}