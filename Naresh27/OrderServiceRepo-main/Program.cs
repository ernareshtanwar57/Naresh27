using Microsoft.Azure.Cosmos;
using OrderService.Repositories;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------------------------------
// Cosmos DB
// ----------------------------------------------------
string cosmosConnectionString =
    builder.Configuration["CosmosDb:ConnectionString"]
    ?? throw new InvalidOperationException(
        "CosmosDb:ConnectionString is missing.");

string databaseName =
    builder.Configuration["CosmosDb:DatabaseName"]
    ?? throw new InvalidOperationException(
        "CosmosDb:DatabaseName is missing.");

string containerName =
    builder.Configuration["CosmosDb:ContainerName"]
    ?? throw new InvalidOperationException(
        "CosmosDb:ContainerName is missing.");

builder.Services.AddSingleton<CosmosClient>(
    new CosmosClient(cosmosConnectionString));

// ----------------------------------------------------
// Repository and Service
// ----------------------------------------------------
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderService, OrderService.Services.OrderService>();



var app = builder.Build();

// ----------------------------------------------------
// Create Cosmos DB database/container if they don't exist
// ----------------------------------------------------
using (IServiceScope scope = app.Services.CreateScope())
{
    CosmosClient cosmosClient =
        scope.ServiceProvider.GetRequiredService<CosmosClient>();

    Database database =
        await cosmosClient.CreateDatabaseIfNotExistsAsync(
            databaseName);

    await database.CreateContainerIfNotExistsAsync(
        containerName,
        "/id");
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
