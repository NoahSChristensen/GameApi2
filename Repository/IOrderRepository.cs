using GameApi2.Models;

namespace GameApi2.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllOrderAsync();

    Task<Order?> GetOrderIdAsync(string id);

    Task<Order> AddOrderAsync(Order order);

    Task<bool> DeleteOrderAsync(string id);

}