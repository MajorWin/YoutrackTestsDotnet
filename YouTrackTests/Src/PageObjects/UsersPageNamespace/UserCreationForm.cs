using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UserCreationForm : PageObject
    {
        public static readonly By UserCreationFormLocator = By.Id("id_l.U.cr.createUserDialog");

        private static readonly By LoginLocator = By.Id("id_l.U.cr.login");
        private static readonly By PasswordLocator = By.Id("id_l.U.cr.password");
        private static readonly By PasswordConfirmationLocator = By.Id("id_l.U.cr.confirmPassword");
        private static readonly By ForcePasswordChangeLocator = By.Id("id_l.U.cr.forcePasswordChange");
        private static readonly By FullNameLocator = By.Id("id_l.U.cr.fullName");
        private static readonly By EmailLocator = By.Id("id_l.U.cr.email");
        private static readonly By JabberLocator = By.Id("id_l.U.cr.jabber");

        private static readonly By OkButtonLocator = By.Id("id_l.U.cr.createUserOk");
        private static readonly By CancelButtonLocator = By.Id("id_l.U.cr.createUserCancel");

        private static readonly By CloseButtonLocator = By.Id("id_l.U.cr.closeCreateUserDlg");

        private static readonly By ErrorBulbLocator = By.CssSelector("div.error-bulb2");
        private static readonly By ErrorBulbMessageLocator = By.CssSelector("div.error-tooltip");

        private readonly UsersPage myParentPage;

        private readonly IWebElement myCreateUserForm;


        public UserCreationForm(IWebDriver browser, UsersPage parentPage) :
            base(browser, ByValidators.ExistsAndDisplayed(UserCreationFormLocator, browser))
        {
            myParentPage = parentPage;
            myCreateUserForm = FindElement(UserCreationFormLocator);
        }


        public string Login => myCreateUserForm.FindElement(LoginLocator).GetAttribute("value");
        public string Password => myCreateUserForm.FindElement(PasswordLocator).GetAttribute("value");
        public string PasswordConfirmation => myCreateUserForm.FindElement(PasswordConfirmationLocator).GetAttribute("value");
        public bool IsPasswordChangeForced => myCreateUserForm.FindElement(ForcePasswordChangeLocator).Selected;
        public string FullName => myCreateUserForm.FindElement(FullNameLocator).GetAttribute("value");
        public string Email => myCreateUserForm.FindElement(EmailLocator).GetAttribute("value");
        public string Jabber => myCreateUserForm.FindElement(JabberLocator).GetAttribute("value");

        public bool Displayed => myCreateUserForm.Displayed;


        [LogAspect]
        public UserCreationForm TypeLogin(string login)
        {
            myCreateUserForm.FindElement(LoginLocator).SendKeys(login);
            return this;
        }

        [LogAspect]
        public UserCreationForm TypePassword(string password)
        {
            myCreateUserForm.FindElement(PasswordLocator).SendKeys(password);
            return this;
        }

        [LogAspect]
        public UserCreationForm TypePasswordConfirmation(string passwordConfirmation)
        {
            myCreateUserForm.FindElement(PasswordConfirmationLocator).SendKeys(passwordConfirmation);
            return this;
        }

        [LogAspect]
        public UserCreationForm ClickForcePasswordChange()
        {
            myCreateUserForm.FindElement(ForcePasswordChangeLocator).Click();
            return this;
        }

        [LogAspect]
        public UserCreationForm TypeFullName(string fullName)
        {
            myCreateUserForm.FindElement(FullNameLocator).SendKeys(fullName);
            return this;
        }

        [LogAspect]
        public UserCreationForm TypeEmail(string email)
        {
            myCreateUserForm.FindElement(EmailLocator).SendKeys(email);
            return this;
        }

        [LogAspect]
        public UserCreationForm TypeJabber(string jabber)
        {
            myCreateUserForm.FindElement(JabberLocator).SendKeys(jabber);
            return this;
        }

        [LogAspect]
        public UserCreationForm ClickOk()
        {
            myCreateUserForm.FindElement(OkButtonLocator).Click();
            return this;
        }

        [LogAspect]
        public UserCreationForm ClickCancel()
        {
            myCreateUserForm.FindElement(CancelButtonLocator).Click();
            return this;
        }

        [LogAspect]
        public UserCreationForm ClickClose()
        {
            myCreateUserForm.FindElement(CloseButtonLocator).Click();
            return this;
        }


        [LogAspect]
        public UserCreationForm Fill(User user)
        {
            TypeLogin(user.Login);
            TypePassword(user.Password);
            TypePasswordConfirmation(user.PasswordConfirmation);
            SetForcedPasswordChange(user.IsPasswordChangeForced);
            TypeFullName(user.FullName);
            TypeEmail(user.Email);
            TypeJabber(user.Jabber);
            return this;
        }

        [LogAspect]
        public UserCreationForm SetForcedPasswordChange(bool isForced)
        {
            var forcePasswordChangeCheckbox = myCreateUserForm.FindElement(ForcePasswordChangeLocator);
            if (forcePasswordChangeCheckbox.Selected != isForced)
            {
                forcePasswordChangeCheckbox.Click();
            }

            return this;
        }

        [LogAspect]
        public EditUserPage SubmitSuccessfully()
        {
            ClickOk();
            return new EditUserPage(Browser);
        }

        [LogAspect]
        public UsersPage Cancel()
        {
            ClickCancel();
            WaitHelpers.Wait(() => !Browser.IsElementDisplayed(UserCreationFormLocator));

            return myParentPage;
        }

        [LogAspect]
        public UsersPage Close()
        {
            ClickClose();
            WaitHelpers.Wait(() => !Browser.IsElementDisplayed(UserCreationFormLocator));

            return myParentPage;
        }

        [LogAspect]
        public string GetErrorBulbMessage()
        {
            var errorBulb = FindElementWithWait(ErrorBulbLocator);
            new Actions(Browser)
                .MoveToElement(errorBulb)
                .MoveByOffset(1, 1)
                .MoveByOffset(-1, -1)
                .Perform(); // move cursor to error bulb
            try
            {
                return FindElementWithWait(ErrorBulbMessageLocator).Text;
            }
            catch (NoSuchElementException e)
            {
                throw e;
            }
        }

        public class User
        {
            private static int myCounter = 0;

            public string Login { get; set; } = "";
            public string Password { get; set; } = "";
            public string PasswordConfirmation { get; set; } = "";
            public bool IsPasswordChangeForced { get; set; } = false;
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Jabber { get; set; } = "";

            public static User CreateFilledUser(
                string login = null,
                string password = null,
                string passwordConfirmation = null,
                bool isPasswordChangeForced = false,
                string fullName = null,
                string email = null,
                string jabber = null)
            {
                myCounter++;

                login ??= $"user{myCounter}";
                password ??= $"password{myCounter}";
                passwordConfirmation ??= $"password{myCounter}";
                fullName ??= $"full name {myCounter}";
                email ??= $"e{myCounter}@mail.com";
                jabber ??= $"j{myCounter}@abber.com";

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
    }
}
