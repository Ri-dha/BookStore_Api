using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.Users.Customers;
using BookStoreTask.Utli;

namespace BookStoreTask.Orders;

public class OrdersModel : BaseEntity<Guid>
{
    public string? Address { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public ShippingStatus OrderStatus { get; set; } = ShippingStatus.Pending;
    public string? Notes { get; set; }
    public string PhoneNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}