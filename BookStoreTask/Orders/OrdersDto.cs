using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.Users.Customers;
using BookStoreTask.Utli;

namespace BookStoreTask.Orders;

public class OrdersDto:BaseDto<Guid>
{
    public string? Address { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public ShippingStatus OrderStatus { get; set; }
    public string? Notes { get; set; }
    public string PhoneNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public List<CartItemDto> CartItems { get; set; }
    
}