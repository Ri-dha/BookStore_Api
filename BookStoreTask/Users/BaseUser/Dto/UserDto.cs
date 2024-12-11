using BookStoreTask.Utli;

namespace BookStoreTask.Users.BaseUser.Dto;

public class UserDto : BaseDto<Guid>
{
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Roles Role { get; set; }
    public string? RoleName => Role.ToString();
    public string Token { get; set; }
    public string? ProfileImage { get; set; }
    public DateTime? LastLogin { get; set; }
}