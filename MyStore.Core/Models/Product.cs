// File: MyStore.Core/Models/Product.cs
using System.ComponentModel.DataAnnotations;

namespace MyStore.Core.Models;

/// <summary>
/// Represents a product in the store
/// JSON Schema: { "id": 1, "name": "string", "description": "string", "price": 99.99, "imageUrl": "string", "stock": 100 }
/// </summary>
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [MaxLength(256, ErrorMessage = "Product name cannot exceed 256 characters")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "ImageUrl must be a valid URL")]
    public string? ImageUrl { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
    public int Stock { get; set; }

    /// <summary>
    /// Example data for development
    /// </summary>
    public static List<Product> GetSampleData()
    {
        return new()
        {
            new Product
            {
                Id = 1,
                Name = "Laptop Dell XPS 13",
                Description = "Ultrabook powerful performance",
                Price = 1299.99m,
                ImageUrl = "https://via.placeholder.com/300x300?text=Laptop+Dell",
                Stock = 15
            },
            new Product
            {
                Id = 2,
                Name = "iPhone 15 Pro",
                Description = "Latest Apple smartphone",
                Price = 999.99m,
                ImageUrl = "https://via.placeholder.com/300x300?text=iPhone+15",
                Stock = 25
            },
            new Product
            {
                Id = 3,
                Name = "Samsung Galaxy S24",
                Description = "Android flagship device",
                Price = 899.99m,
                ImageUrl = "https://via.placeholder.com/300x300?text=Samsung+S24",
                Stock = 30
            },
            new Product
            {
                Id = 4,
                Name = "Sony WH-1000XM5 Headphones",
                Description = "Premium noise-canceling headphones",
                Price = 399.99m,
                ImageUrl = "https://via.placeholder.com/300x300?text=Sony+Headphones",
                Stock = 50
            },
            new Product
            {
                Id = 5,
                Name = "Apple AirPods Pro",
                Description = "Wireless earbuds with ANC",
                Price = 249.99m,
                ImageUrl = "https://via.placeholder.com/300x300?text=AirPods+Pro",
                Stock = 40
            }
        };
    }
}

/// <summary>
/// DTO for API responses
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }

    public Product ToEntity()
    {
        return new Product
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Price = Price,
            ImageUrl = ImageUrl,
            Stock = Stock
        };
    }

    public static ProductDto FromEntity(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock
        };
    }
}
