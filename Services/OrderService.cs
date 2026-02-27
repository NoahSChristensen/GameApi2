using GameApi2.DTOs;
using GameApi2.Models;
using GameApi2.Repositories;

namespace GameApi2.Services;

public class OrderService
{

    private readonly IOrderRepository _orderRepository;


    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public async Task<List<OrderGetDto>> GetAllOrderAsync()
    {
        var orders = await _orderRepository.GetAllOrderAsync();
        return orders.Select(MapToGetDto).ToList();
    }

    public async Task<OrderGetDto> CreateOrderAsync(OrderPostDTO orderPostDTO, User user)
    {
        Order order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            User = user,
            Title = orderPostDTO.Title,
            OrderValue = orderPostDTO.OrderValue,
            Oprettet = DateTime.UtcNow,
            Opdateret = DateTime.UtcNow
        };


        var createdOrder = await _orderRepository.AddOrderAsync(order);
        return MapToGetDto(createdOrder);
    }

    public async Task<bool> DeleteOrderAsync(string id, OrderDeleteDto? deleteDto = null)
    {
        return await _orderRepository.DeleteOrderAsync(id);
    }


    private static OrderGetDto MapToGetDto(Order order)
    {
        return new OrderGetDto
        {
            Id = Guid.NewGuid().ToString(),
            UserId = order.UserId,
            User = order.User,
            Title = order.Title,
            Oprettet = order.Oprettet,
            Opdateret = order.Opdateret
        };
    }

    internal async Task<object?> CreateOrderAsync(OrderPostDTO orderPostDTO)
    {
        throw new NotImplementedException();
    }
}