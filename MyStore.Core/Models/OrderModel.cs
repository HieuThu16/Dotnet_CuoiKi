// File: MyStore.Core/Models/OrderModel.cs
using System.ComponentModel.DataAnnotations;

namespace MyStore.Core.Models;

/// <summary>
/// Represents a customer order
/// JSON Schema:
/// {
///   "orderId": "guid-string",
///   "customerName": "string",
///   "customerAddress": "string",
///   "customerPhone": "string",
///   "items": [{ "productId": 1, "name": "string", "unitPrice": 99.99, "quantity": 1 }],
///   "total": 99.99,
///   "createdAt": "2024-01-01T00:00:00Z"
/// }
/// </summary>
public class OrderModel
{
    [Key]
    public string OrderId { get; set; } = Guid.NewGuid().ToString("N");

    [Required(ErrorMessage = "Customer name is required")]
    [MaxLength(256, ErrorMessage = "Customer name cannot exceed 256 characters")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [MaxLength(512, ErrorMessage = "Address cannot exceed 512 characters")]
    public string CustomerAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\+?[\d\s\-\(\)]{10,20}$", ErrorMessage = "Invalid phone number")]
    [MaxLength(20)]
    public string CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// List of order items (serialized as JSON)
    /// </summary>
    public List<OrderItemDto> Items { get; set; } = new();

    /// <summary>
    /// Total order amount
    /// </summary>
    [Range(0.01, 999999.99)]
    public decimal Total { get; set; }

    /// <summary>
    /// Order creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Order status: Pending, Processing, Completed, Cancelled
    /// </summary>
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Notes or special instructions
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Create order from cart items
    /// </summary>
    public static OrderModel CreateFromCart(
        List<CartItem> cartItems,
        string customerName,
        string customerAddress,
        string customerPhone)
    {
        if (cartItems?.Count == 0)
            throw new ArgumentException("Cart cannot be empty", nameof(cartItems));

        var order = new OrderModel
        {
            CustomerName = customerName,
            CustomerAddress = customerAddress,
            CustomerPhone = customerPhone,
            Items = cartItems.Select(ci => new OrderItemDto
            {
                ProductId = ci.ProductId,
                Name = ci.Name,
                UnitPrice = ci.UnitPrice,
                Quantity = ci.Quantity
            }).ToList(),
            Total = cartItems.Sum(ci => ci.Subtotal),
            CreatedAt = DateTime.UtcNow
        };

        return order;
    }

    public override string ToString()
    {
        return $"Order {OrderId}: {CustomerName}, ${Total}, {Items.Count} items";
    }
}

/// <summary>
/// DTO for order items in JSON
/// </summary>
public class OrderItemDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
}

/// <summary>
/// API Response wrapper
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse SuccessResponse(string message = "Success", object? data = null)
    {
        return new ApiResponse { Success = true, Message = message, Data = data };
    }

    public static ApiResponse ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? new() { message }
        };
    }
}

/// <summary>
/// For API calls
/// </summary>
public class OrderDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal Total { get; set; }
    public string? Notes { get; set; }

    public static OrderDto FromOrderModel(OrderModel order)
    {
        return new OrderDto
        {
            CustomerName = order.CustomerName,
            CustomerAddress = order.CustomerAddress,
            CustomerPhone = order.CustomerPhone,
            Items = order.Items,
            Total = order.Total,
            Notes = order.Notes
        };
    }
}
