// File: MyStore.Core/Services/CartService.cs
using MyStore.Core.Data;
using MyStore.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Core.Services;

/// <summary>
/// Implementation of ICartService using EF Core
/// Manages shopping cart persistence with SQLite
/// </summary>
public class CartService : ICartService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly object _lockObject = new();

    public event EventHandler<CartChangedEventArgs>? CartChanged;

    public CartService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    /// <summary>
    /// Add a product to cart with quantity
    /// </summary>
    public async Task<CartItem> AddItemAsync(Product product, int quantity = 1)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (quantity < 1)
            throw new ArgumentException("Quantity must be at least 1", nameof(quantity));

        if (quantity > product.Stock)
            throw new ArgumentException($"Quantity exceeds available stock ({product.Stock})", nameof(quantity));

        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();

            // Check if product already exists in cart
            var existingItem = context.CartItems
                .FirstOrDefault(c => c.ProductId == product.Id);

            CartItem cartItem;

            if (existingItem != null)
            {
                // Update existing item quantity
                var newQuantity = existingItem.Quantity + quantity;
                if (newQuantity > product.Stock)
                    throw new ArgumentException($"Total quantity exceeds available stock ({product.Stock})", nameof(quantity));

                existingItem.Quantity = newQuantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
                context.CartItems.Update(existingItem);
                cartItem = existingItem;
            }
            else
            {
                // Create new cart item
                cartItem = CartItem.CreateFromProduct(product, quantity);
                context.CartItems.Add(cartItem);
            }

            context.SaveChanges();

            OnCartChanged(CartChangeType.ItemAdded, cartItem,
                existingItem != null ? "Quantity updated" : "Item added to cart");

            return cartItem;
        }
    }

    /// <summary>
    /// Remove a cart item by ID
    /// </summary>
    public async Task<bool> RemoveItemAsync(int cartItemId)
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var item = context.CartItems.Find(cartItemId);
            if (item == null)
                return false;

            context.CartItems.Remove(item);
            context.SaveChanges();

            OnCartChanged(CartChangeType.ItemRemoved, item, "Item removed from cart");
            return true;
        }
    }

    /// <summary>
    /// Update cart item quantity
    /// </summary>
    public async Task<CartItem?> UpdateItemAsync(int cartItemId, int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var item = context.CartItems.Find(cartItemId);
            if (item == null)
                return null;

            if (newQuantity == 0)
            {
                context.CartItems.Remove(item);
                context.SaveChanges();
                OnCartChanged(CartChangeType.ItemRemoved, item, "Item removed (quantity set to 0)");
                return null;
            }

            item.Quantity = newQuantity;
            item.UpdatedAt = DateTime.UtcNow;
            context.CartItems.Update(item);
            context.SaveChanges();

            OnCartChanged(CartChangeType.QuantityChanged, item,
                $"Quantity updated to {newQuantity}");

            return item;
        }
    }

    /// <summary>
    /// Get all cart items
    /// </summary>
    public async Task<List<CartItem>> GetCartAsync()
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.CartItems
                .OrderBy(c => c.CreatedAt)
                .ToList();
        }
    }

    /// <summary>
    /// Get single cart item
    /// </summary>
    public async Task<CartItem?> GetCartItemAsync(int cartItemId)
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.CartItems.FirstOrDefault(c => c.Id == cartItemId);
        }
    }

    /// <summary>
    /// Clear all cart items
    /// </summary>
    public async Task ClearCartAsync()
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();

            OnCartChanged(CartChangeType.CartCleared, null, "Cart cleared");
        }
    }

    /// <summary>
    /// Get total quantity in cart
    /// </summary>
    public async Task<int> GetTotalQuantityAsync()
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.CartItems.Sum(c => c.Quantity);
        }
    }

    /// <summary>
    /// Get total price in cart
    /// </summary>
    public async Task<decimal> GetTotalPriceAsync()
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();
            // SQLite doesn't support Sum on decimal, so we load and calculate on client side
            var items = context.CartItems.ToList();
            return items.Sum(c => c.UnitPrice * c.Quantity);
        }
    }

    /// <summary>
    /// Check if cart is empty
    /// </summary>
    public async Task<bool> IsEmptyAsync()
    {
        lock (_lockObject)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return !context.CartItems.Any();
        }
    }

    /// <summary>
    /// Raise CartChanged event
    /// </summary>
    private void OnCartChanged(CartChangeType changeType, CartItem? item, string message)
    {
        CartChanged?.Invoke(this, new CartChangedEventArgs
        {
            ChangeType = changeType,
            Item = item,
            Message = message
        });
    }
}
