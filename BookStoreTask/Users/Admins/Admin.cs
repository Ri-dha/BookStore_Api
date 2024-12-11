using BookStoreTask.Users.BaseUser;

namespace BookStoreTask.Users.Admins;

public class Admin:User
{
    public AdministrativeRoles? AdministrativeRole { get; set; }
}