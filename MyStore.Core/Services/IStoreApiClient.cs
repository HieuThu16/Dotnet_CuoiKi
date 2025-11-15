// File: MyStore.Core/Services/IStoreApiClient.cs
using Refit;
using MyStore.Core.Models;

namespace MyStore.Core.Services;

/// <summary>
/// Refit API client interface for backend communication
/// Base URL: https://api.example.com (configurable)
/// </summary>
[Headers("Accept: application/json", "Content-Type: application/json")]
public interface IStoreApiClient
{
    /// <summary>
    /// GET /api/products
    /// Fetch all products from API
    /// </summary>
    [Get("/api/products")]
    Task<ApiResponse<List<ProductDto>>> GetProductsAsync();

    /// <summary>
    /// GET /api/products/{id}
    /// Fetch single product by ID
    /// </summary>
    [Get("/api/products/{id}")]
    Task<ApiResponse<ProductDto>> GetProductAsync(int id);

    /// <summary>
    /// POST /api/orders
    /// Create new order
    /// </summary>
    [Post("/api/orders")]
    Task<ApiResponse<OrderResponseDto>> PostOrderAsync([Body] OrderDto order);

    /// <summary>
    /// GET /api/orders/{orderId}
    /// Get order status
    /// </summary>
    [Get("/api/orders/{orderId}")]
    Task<ApiResponse<OrderStatusDto>> GetOrderStatusAsync(string orderId);
}

/// <summary>
/// Generic API response wrapper
/// </summary>
public class ApiResponse<T> where T : class
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Order response from API
/// </summary>
public class OrderResponseDto
{
    public string OrderId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal Total { get; set; }
}

/// <summary>
/// Order status response
/// </summary>
public class OrderStatusDto
{
    public string OrderId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Pending, Processing, Completed, Cancelled
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Client for mock API (in-memory testing)
/// </summary>
public class MockStoreApiClient : IStoreApiClient
{
    private static readonly List<ProductDto> MockProducts = new()
    {
        new ProductDto { Id = 1, Name = "Laptop Dell XPS 13", Description = "Ultrabook", Price = 1299.99m, Stock = 15, ImageUrl = "https://via.placeholder.com/300x300?text=Laptop" },
        new ProductDto { Id = 2, Name = "iPhone 15 Pro", Description = "Apple smartphone", Price = 999.99m, Stock = 25, ImageUrl = "https://via.placeholder.com/300x300?text=iPhone" },
        new ProductDto { Id = 3, Name = "Samsung Galaxy S24", Description = "Android flagship", Price = 899.99m, Stock = 30, ImageUrl = "https://via.placeholder.com/300x300?text=Samsung" },
        new ProductDto { Id = 4, Name = "Sony WH-1000XM5", Description = "Headphones", Price = 399.99m, Stock = 50, ImageUrl = "https://via.placeholder.com/300x300?text=Sony" },
        new ProductDto { Id = 5, Name = "Apple AirPods Pro", Description = "Earbuds", Price = 249.99m, Stock = 40, ImageUrl = "https://via.placeholder.com/300x300?text=AirPods" }
    };

    public Task<ApiResponse<List<ProductDto>>> GetProductsAsync()
    {
        return Task.FromResult(new ApiResponse<List<ProductDto>>
        {
            Success = true,
            Message = "Products fetched successfully",
            Data = MockProducts
        });
    }

    public Task<ApiResponse<ProductDto>> GetProductAsync(int id)
    {
        var product = MockProducts.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return Task.FromResult(new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "Product not found",
                Errors = new() { "Product not found" }
            });
        }

        return Task.FromResult(new ApiResponse<ProductDto>
        {
            Success = true,
            Message = "Product fetched successfully",
            Data = product
        });
    }

    public Task<ApiResponse<OrderResponseDto>> PostOrderAsync(OrderDto order)
    {
        var response = new OrderResponseDto
        {
            OrderId = Guid.NewGuid().ToString("N"),
            Message = "Order created successfully",
            CreatedAt = DateTime.UtcNow,
            Total = order.Total
        };

        return Task.FromResult(new ApiResponse<OrderResponseDto>
        {
            Success = true,
            Message = "Order placed successfully",
            Data = response
        });
    }

    public Task<ApiResponse<OrderStatusDto>> GetOrderStatusAsync(string orderId)
    {
        return Task.FromResult(new ApiResponse<OrderStatusDto>
        {
            Success = true,
            Message = "Order status retrieved",
            Data = new OrderStatusDto
            {
                OrderId = orderId,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        });
    }
}
