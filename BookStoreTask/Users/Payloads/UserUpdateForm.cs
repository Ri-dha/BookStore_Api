using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser;
using BookStoreTask.Users.Customers;

namespace BookStoreTask.Users.Payloads;

public class UserUpdateForm
{
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public IFormFile? ProfileImage { get; set; }
}


public class AdminUpdateForm : UserUpdateForm
{
    public AdministrativeRoles? AdministrativeRole { get; set; }
}

public class CustomerUpdateForm : UserUpdateForm
{
    public CustomerStatus? CustomerStatus { get; set; }
}