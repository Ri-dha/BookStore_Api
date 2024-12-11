using BookStoreTask.Users.BaseUser.Dto;

namespace BookStoreTask.Users.Admins;

public class AdminDto: UserDto
{
    public AdministrativeRoles? AdministrativeRole { get; set; }
    public string? AdministrativeRoleName => AdministrativeRole?.ToString();   
}