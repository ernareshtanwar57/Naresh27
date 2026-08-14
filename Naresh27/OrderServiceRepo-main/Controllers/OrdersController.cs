using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderRequest request)
        {
            try
            {
                Order order =
                    await _orderService.CreateOrderAsync(request);

                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { id = order.Id },
                    order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            List<Order> orders =
                await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(
            string id)
        {
            Order? order =
                await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound(
                    $"Order with id '{id}' was not found.");
            }

            return Ok(order);
        }

        // PUT: api/orders/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
            string id,
            [FromBody] UpdateOrderRequest request)
        {
            try
            {
                Order? order =
                    await _orderService.UpdateOrderStatusAsync(
                        id,
                        request);

                if (order == null)
                {
                    return NotFound(
                        $"Order with id '{id}' was not found.");
                }

                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
    }
}
