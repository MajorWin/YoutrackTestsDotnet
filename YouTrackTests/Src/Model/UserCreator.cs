namespace YouTrackWebdriverTests.Model;

public static class UserCreator
{
    private static int _counter = 0;
    
    public static User CreateFilledUser(
        string login = null,
        string password = null,
        string passwordConfirmation = null,
        bool isPasswordChangeForced = false,
        string fullName = null,
        string email = null,
        string jabber = null)
    {
        _counter++;

        login ??= $"user{_counter}";
        password ??= $"password{_counter}";
        passwordConfirmation ??= $"password{_counter}";
        fullName ??= $"full name {_counter}";
        email ??= $"e{_counter}@mail.com";
        jabber ??= $"j{_counter}@jabber.com";

        return new User
        {
            Login = login,
            Password = password,
            PasswordConfirmation = passwordConfirmation,
            IsPasswordChangeForced = isPasswordChangeForced,
            FullName = fullName,
            Email = email,
            Jabber = jabber
        };
    }
}