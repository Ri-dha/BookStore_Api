using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.Users.BaseUser;

public class User: BaseEntity<Guid>
{
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public Roles? Role { get; set; }
    public DateTime? LastLogin { get; set; }
    public ProjectFiles ProfileImage { get; set; }
}