using Microsoft.Azure.Cosmos;
using OrderService.Models;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        private readonly Container _container;

        public OrderRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            string databaseName =
                configuration["CosmosDb:DatabaseName"]
                ?? throw new InvalidOperationException("CosmosDb:DatabaseName is missing.");

            string containerName =
                configuration["CosmosDb:ContainerName"]
                ?? throw new InvalidOperationException("CosmosDb:ContainerName is missing.");

            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task<Order> CreateAsync(Order order)
        {
            //throw new NotImplementedException();

            ItemResponse<Order> response =
               await _container.CreateItemAsync(
                   order,
                   new PartitionKey(order.Id));

            return response.Resource;   
        }

        public async Task<bool> DeleteAsync(string id)
        {
            //throw new NotImplementedException();

            try
            {
                await _container.DeleteItemAsync<Order>(
                    id,
                    new PartitionKey(id));

                return true;
            }
            catch (CosmosException ex)
                when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
        

        public async Task<List<Order>> GetAllAsync()
        {
            //throw new NotImplementedException();
            List<Order> orders = new();

            QueryDefinition query =
                new QueryDefinition("SELECT * FROM c");

            using FeedIterator<Order> result =
                _container.GetItemQueryIterator<Order>(query);

            while (result.HasMoreResults)
            {
                FeedResponse<Order> response =
                    await result.ReadNextAsync();

                orders.AddRange(response);
            }

            return orders;
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            //throw new NotImplementedException();

            try
            {
                ItemResponse<Order> response =
                    await _container.ReadItemAsync<Order>(
                        id,
                        new PartitionKey(id));

                return response.Resource;
            }
            catch (CosmosException ex)
                when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Order?> UpdateStatusAsync(string id, string status)
        {
            //throw new NotImplementedException();

            Order? existingOrder = await GetByIdAsync(id);

            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.Status = status;

            ItemResponse<Order> response =
                await _container.ReplaceItemAsync(
                    existingOrder,
                    id,
                    new PartitionKey(id));

            return response.Resource;
        }
    }
}
