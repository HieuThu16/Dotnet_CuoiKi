// File: MyStore.Core/Models/CartItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStore.Core.Models;

/// <summary>
/// Represents an item in the shopping cart
/// JSON Schema: { "id": 1, "productId": 1, "name": "string", "unitPrice": 99.99, "quantity": 1, "subtotal": 99.99 }
/// </summary>
public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "ProductId is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 999999.99)]
    public decimal UnitPrice { get; set; }

    [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10000")]
    public int Quantity { get; set; }

    /// <summary>
    /// Calculated property: UnitPrice * Quantity
    /// </summary>
    [NotMapped]
    public decimal Subtotal => UnitPrice * Quantity;

    /// <summary>
    /// Timestamp for tracking when item was added
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last updated timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Create a CartItem from a Product
    /// </summary>
    public static CartItem CreateFromProduct(Product product, int quantity = 1)
    {
        if (quantity < 1)
            throw new ArgumentException("Quantity must be at least 1", nameof(quantity));

        if (quantity > product.Stock)
            throw new ArgumentException($"Quantity exceeds available stock ({product.Stock})", nameof(quantity));

        return new CartItem
        {
            ProductId = product.Id,
            Name = product.Name,
            UnitPrice = product.Price,
            Quantity = quantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public override string ToString()
    {
        return $"{Name} x{Quantity} @ ${UnitPrice} = ${Subtotal}";
    }
}
