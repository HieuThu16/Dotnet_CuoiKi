// File: MyStore.Core/Services/ICartService.cs
using MyStore.Core.Models;

namespace MyStore.Core.Services;

/// <summary>
/// Service interface for cart operations
/// Manages cart items, calculations, and persistence
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Add a product to cart or increase quantity if already exists
    /// </summary>
    /// <param name="product">Product to add</param>
    /// <param name="quantity">Quantity to add</param>
    /// <returns>Added CartItem</returns>
    Task<CartItem> AddItemAsync(Product product, int quantity = 1);

    /// <summary>
    /// Remove a cart item by id
    /// </summary>
    /// <param name="cartItemId">CartItem ID to remove</param>
    /// <returns>True if removed, false if not found</returns>
    Task<bool> RemoveItemAsync(int cartItemId);

    /// <summary>
    /// Update quantity of a cart item
    /// </summary>
    /// <param name="cartItemId">CartItem ID to update</param>
    /// <param name="newQuantity">New quantity value</param>
    /// <returns>Updated CartItem</returns>
    Task<CartItem?> UpdateItemAsync(int cartItemId, int newQuantity);

    /// <summary>
    /// Get all items in cart
    /// </summary>
    /// <returns>List of CartItems</returns>
    Task<List<CartItem>> GetCartAsync();

    /// <summary>
    /// Get single cart item by id
    /// </summary>
    /// <param name="cartItemId">CartItem ID</param>
    /// <returns>CartItem or null</returns>
    Task<CartItem?> GetCartItemAsync(int cartItemId);

    /// <summary>
    /// Clear all items from cart
    /// </summary>
    /// <returns>Completed task</returns>
    Task ClearCartAsync();

    /// <summary>
    /// Get total quantity of items in cart
    /// </summary>
    /// <returns>Total quantity</returns>
    Task<int> GetTotalQuantityAsync();

    /// <summary>
    /// Get total price of items in cart
    /// </summary>
    /// <returns>Total price</returns>
    Task<decimal> GetTotalPriceAsync();

    /// <summary>
    /// Check if cart is empty
    /// </summary>
    /// <returns>True if empty</returns>
    Task<bool> IsEmptyAsync();

    /// <summary>
    /// Event fired when cart changes
    /// </summary>
    event EventHandler<CartChangedEventArgs>? CartChanged;
}

/// <summary>
/// Event args for cart changes
/// </summary>
public class CartChangedEventArgs : EventArgs
{
    public CartChangeType ChangeType { get; set; }
    public CartItem? Item { get; set; }
    public string Message { get; set; } = string.Empty;
}

public enum CartChangeType
{
    ItemAdded,
    ItemRemoved,
    ItemUpdated,
    CartCleared,
    QuantityChanged
}
