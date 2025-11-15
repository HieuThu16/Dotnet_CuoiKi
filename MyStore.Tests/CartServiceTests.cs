using Xunit;
using MyStore.Core.Models;

namespace MyStore.Tests;

public class CartServiceTests
{
    [Fact]
    public void CartItem_CreateFromProduct_ShouldInitializeCorrectly()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 100m,
            Stock = 10
        };

        var cartItem = CartItem.CreateFromProduct(product, 2);

        Assert.Equal(1, cartItem.ProductId);
        Assert.Equal("Test Product", cartItem.Name);
        Assert.Equal(100m, cartItem.UnitPrice);
        Assert.Equal(2, cartItem.Quantity);
        Assert.Equal(200m, cartItem.Subtotal);
    }

    [Fact]
    public void CartItem_Subtotal_ShouldCalculateCorrectly()
    {
        var cartItem = new CartItem
        {
            Id = 1,
            ProductId = 1,
            Name = "Product",
            UnitPrice = 50m,
            Quantity = 3
        };

        Assert.Equal(150m, cartItem.Subtotal);
    }

    [Fact]
    public void OrderModel_CreateFromCart_ShouldBuildCorrectOrder()
    {
        var cartItems = new List<CartItem>
        {
            new CartItem { ProductId = 1, Name = "Product 1", UnitPrice = 100m, Quantity = 1 },
            new CartItem { ProductId = 2, Name = "Product 2", UnitPrice = 200m, Quantity = 2 }
        };

        var order = OrderModel.CreateFromCart(
            cartItems,
            "John Doe",
            "123 Main St",
            "1234567890"
        );

        Assert.Equal("John Doe", order.CustomerName);
        Assert.Equal("123 Main St", order.CustomerAddress);
        Assert.Equal("1234567890", order.CustomerPhone);
        Assert.Equal(500m, order.Total);
        Assert.Equal(2, order.Items.Count);
    }

    [Fact]
    public void OrderModel_CreateFromCart_WithEmptyCart_ShouldThrow()
    {
        var cartItems = new List<CartItem>();

        Assert.Throws<ArgumentException>(() =>
            OrderModel.CreateFromCart(cartItems, "Name", "Address", "Phone")
        );
    }

    [Fact]
    public void CartItem_CreateFromProduct_WithExcessiveQuantity_ShouldThrow()
    {
        var product = new Product { Id = 1, Name = "Product", Price = 100m, Stock = 5 };

        Assert.Throws<ArgumentException>(() =>
            CartItem.CreateFromProduct(product, 10)
        );
    }

    [Fact]
    public void Product_GetSampleData_ShouldReturnProducts()
    {
        var products = Product.GetSampleData();

        Assert.NotEmpty(products);
        Assert.True(products.Count >= 5);
        Assert.All(products, p => Assert.NotEmpty(p.Name));
        Assert.All(products, p => Assert.True(p.Price > 0));
    }

    [Fact]
    public void ProductDto_ToEntity_ShouldMapCorrectly()
    {
        var dto = new ProductDto
        {
            Id = 1,
            Name = "Test",
            Price = 99.99m,
            Stock = 10
        };

        var entity = dto.ToEntity();

        Assert.Equal(dto.Id, entity.Id);
        Assert.Equal(dto.Name, entity.Name);
        Assert.Equal(dto.Price, entity.Price);
        Assert.Equal(dto.Stock, entity.Stock);
    }

    [Fact]
    public void ProductDto_FromEntity_ShouldMapCorrectly()
    {
        var entity = new Product
        {
            Id = 1,
            Name = "Test",
            Price = 99.99m,
            Stock = 10
        };

        var dto = ProductDto.FromEntity(entity);

        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.Name, dto.Name);
        Assert.Equal(entity.Price, dto.Price);
        Assert.Equal(entity.Stock, dto.Stock);
    }

    [Fact]
    public void OrderModel_ToString_ShouldFormatCorrectly()
    {
        var order = new OrderModel
        {
            OrderId = "order123",
            CustomerName = "John",
            Total = 500m
        };
        order.Items.Add(new OrderItemDto { Name = "Item1", Quantity = 1 });

        var str = order.ToString();

        Assert.Contains("order123", str);
        Assert.Contains("John", str);
        Assert.Contains("$500", str);
    }

    [Fact]
    public void ApiResponse_SuccessResponse_ShouldCreateSuccess()
    {
        var response = ApiResponse.SuccessResponse("Test message", new { value = 42 });

        Assert.True(response.Success);
        Assert.Equal("Test message", response.Message);
        Assert.NotNull(response.Data);
    }

    [Fact]
    public void ApiResponse_ErrorResponse_ShouldCreateError()
    {
        var errors = new List<string> { "Error 1", "Error 2" };
        var response = ApiResponse.ErrorResponse("Failed", errors);

        Assert.False(response.Success);
        Assert.Equal("Failed", response.Message);
        Assert.Equal(2, response.Errors.Count);
    }
}
