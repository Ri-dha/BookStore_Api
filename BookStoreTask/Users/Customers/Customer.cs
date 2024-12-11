using BookStoreTask.Users.BaseUser;

namespace BookStoreTask.Users.Customers;

public class Customer:User
{
    public CustomerStatus CustomerStatus { get; set; }
}