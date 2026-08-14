using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);

        Task<List<Order>> GetAllOrdersAsync();

        Task<Order?> GetOrderByIdAsync(string id);

        Task<Order?> UpdateOrderStatusAsync( string id, UpdateOrderRequest request);

        Task<bool> DeleteOrderAsync(string id);
    }
}
