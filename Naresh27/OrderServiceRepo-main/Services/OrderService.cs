using OrderService.DTOs;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }
        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            //throw new NotImplementedException();

            if (request.Quantity <= 0)
            {
                throw new ArgumentException(
                    "Quantity must be greater than zero.");
            }

            if (request.UnitPrice < 0)
            {
                throw new ArgumentException(
                    "Unit price cannot be negative.");
            }

            Order order = new Order
            {
                Id = Guid.NewGuid().ToString(),

                CustomerId = request.CustomerId,

                ProductId = request.ProductId,

                ProductName = request.ProductName,

                Quantity = request.Quantity,

                UnitPrice = request.UnitPrice,

                TotalAmount =
                    request.Quantity * request.UnitPrice,

                Status = "Placed",

                CreatedAt = DateTime.UtcNow
            };

            return await _repository.CreateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            //throw new NotImplementedException();

            return await _repository.DeleteAsync(id);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            //throw new NotImplementedException();

            return await _repository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            //throw new NotImplementedException();
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Order?> UpdateOrderStatusAsync(string id, UpdateOrderRequest request)
        {
            //throw new NotImplementedException();

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                throw new ArgumentException(
                    "Order status is required.");
            }

            return await _repository.UpdateStatusAsync(
                id,
                request.Status);
        }
    }
}
