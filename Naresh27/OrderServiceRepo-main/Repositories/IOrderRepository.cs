using OrderService.Models;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);

        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(string id);

        Task<Order?> UpdateStatusAsync(string id, string status);

        Task<bool> DeleteAsync(string id);
    }
}
