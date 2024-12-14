using BookStoreTask.Cart;
using BookStoreTask.Users.BaseUser;

namespace BookStoreTask.Users.Customers;

public class Customer:User
{
    public CustomerStatus CustomerStatus { get; set; }
    public Guid? CartId { get; set; }
    public Carts? Cart { get; set; }
}