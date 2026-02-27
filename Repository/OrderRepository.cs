using GameApi2.Data;
using GameApi2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameApi2.Repositories;

public class OrderRepository : IOrderRepository
{

    private readonly DbContextGameApi _dbContext;

    public OrderRepository(DbContextGameApi dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetAllOrderAsync()
    {

        var orders = await _dbContext.Orders.ToListAsync();

        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {

        return await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> AddOrderAsync(Order order)
    {

        if (string.IsNullOrWhiteSpace(order.Id)) order.Id = Guid.NewGuid().ToString();

        order.Oprettet = DateTime.UtcNow;
        order.Opdateret = DateTime.UtcNow;

        _dbContext.Orders.Add(order);

        await _dbContext.SaveChangesAsync();

        return order;
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {

        Order? existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);

        if (existingOrder == null) return false;

        _dbContext.Orders.Remove(existingOrder);
        await _dbContext.SaveChangesAsync();


        return true;

    }

    public Task<Order?> GetOrderIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}