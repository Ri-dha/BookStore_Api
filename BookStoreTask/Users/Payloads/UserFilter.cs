using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser;
using BookStoreTask.Users.Customers;
using BookStoreTask.Utli;

namespace BookStoreTask.Users.Payloads;

public class UserFilter : BaseFilter
{
    public string? Email { get; set; }

    public string? UserName { get; set; }
    public Roles? Role { get; set; }
    
    public string? PhoneNumber { get; set; }
    
}


public class CustomerFilter:UserFilter
{
    
    public CustomerStatus? CustomerStatus { get; set; }
    
    
}

public class AdminFilter:UserFilter
{
    
    public AdministrativeRoles? AdministrativeRole { get; set; }    
    
}
