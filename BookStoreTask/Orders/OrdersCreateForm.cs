using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.Utli;

namespace BookStoreTask.Orders;

public class OrdersCreateForm
{
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public string PhoneNumber { get; set; }
    public Guid? CustomerId { get; set; }
    
}

public class OrderFilterForm:BaseFilter
{
    public ShippingStatus? OrderStatus { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? CustomerId { get; set; }
}