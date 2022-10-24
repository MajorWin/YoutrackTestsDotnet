namespace YouTrackWebdriverTests.Model;

public class User
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public string PasswordConfirmation { get; set; } = "";
    public bool IsPasswordChangeForced { get; set; } = false;
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Jabber { get; set; } = "";
}