using System.ComponentModel.DataAnnotations;
using BookStoreTask.Users.Admins;
using BookStoreTask.Users.Customers;

namespace BookStoreTask.Users.Payloads;

public class UserForm
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
    
    public IFormFile? ProfileImage { get; set; }
}

public class AdminForm : UserForm
{
    [Required(ErrorMessage = "Administrative Role is required.")]
    public AdministrativeRoles AdministrativeRole { get; set; }
}

public class CustomerForm : UserForm
{
}