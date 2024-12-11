namespace BookStoreTask.Users.Payloads;

public class ChangePasswordForm
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}