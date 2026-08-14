using Newtonsoft.Json;

namespace OrderService.Models
{
    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CustomerId { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Placed";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
