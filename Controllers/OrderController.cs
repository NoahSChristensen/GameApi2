using GameApi2.Data;
using GameApi2.DTOs;
using GameApi2.Models;
using GameApi2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameApi2.Controllers;

public static class OrderController
{

    public static async Task MapOrderEndPoints(this WebApplication app)


    {
        app.MapGet("/orders", async (OrderService orderService) =>
        {
            var orders = await orderService.GetAllOrderAsync();
            return Results.Ok(orders);
        }).WithName("GetAllOrders");


        app.MapPost("/orders", async ([FromBody] OrderPostDTO orderPostDTO, OrderService orderService, DbContextGameApi dbContext) =>
        {

            var user = await dbContext.Users.FindAsync(orderPostDTO.UserId);

            if (user == null) return Results.BadRequest("User not found");


            var order = await orderService.CreateOrderAsync(orderPostDTO, user);
            return Results.Created($"/orders/{orderPostDTO.UserId}", order);
        }).WithName("CreateOrder");
    }

}