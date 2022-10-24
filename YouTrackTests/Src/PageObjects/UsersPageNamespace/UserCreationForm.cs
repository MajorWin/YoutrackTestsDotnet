using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using YouTrackWebdriverTests.Model;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UserCreationForm : YoutrackPageObject
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

        private readonly UsersPage _parentPage;

        private readonly IWebElement _userCreationFormElement;


        public UserCreationForm(IWebDriver browser, UsersPage parentPage) :
            base(browser, new WebElementExistsAndDisplayedValidator(browser, UserCreationFormLocator))
        {
            _parentPage = parentPage;
            _userCreationFormElement = Browser.FindElement(UserCreationFormLocator);
        }


        public string Login => _userCreationFormElement.FindElement(LoginLocator).GetAttribute("value");
        public string Password => _userCreationFormElement.FindElement(PasswordLocator).GetAttribute("value");
        public string PasswordConfirmation => _userCreationFormElement.FindElement(PasswordConfirmationLocator).GetAttribute("value");
        public bool IsPasswordChangeForced => _userCreationFormElement.FindElement(ForcePasswordChangeLocator).Selected;
        public string FullName => _userCreationFormElement.FindElement(FullNameLocator).GetAttribute("value");
        public string Email => _userCreationFormElement.FindElement(EmailLocator).GetAttribute("value");
        public string Jabber => _userCreationFormElement.FindElement(JabberLocator).GetAttribute("value");

        public bool Displayed => _userCreationFormElement.Displayed;


        // [LogAspect]
        public UserCreationForm TypeLogin(string login)
        {
            _userCreationFormElement.FindElement(LoginLocator).SendKeys(login);
            return this;
        }

        // [LogAspect]
        public UserCreationForm TypePassword(string password)
        {
            _userCreationFormElement.FindElement(PasswordLocator).SendKeys(password);
            return this;
        }

        // [LogAspect]
        public UserCreationForm TypePasswordConfirmation(string passwordConfirmation)
        {
            _userCreationFormElement.FindElement(PasswordConfirmationLocator).SendKeys(passwordConfirmation);
            return this;
        }

        // [LogAspect]
        public UserCreationForm ClickForcePasswordChange()
        {
            _userCreationFormElement.FindElement(ForcePasswordChangeLocator).Click();
            return this;
        }

        // [LogAspect]
        public UserCreationForm TypeFullName(string fullName)
        {
            _userCreationFormElement.FindElement(FullNameLocator).SendKeys(fullName);
            return this;
        }

        // [LogAspect]
        public UserCreationForm TypeEmail(string email)
        {
            _userCreationFormElement.FindElement(EmailLocator).SendKeys(email);
            return this;
        }

        // [LogAspect]
        public UserCreationForm TypeJabber(string jabber)
        {
            _userCreationFormElement.FindElement(JabberLocator).SendKeys(jabber);
            return this;
        }

        // [LogAspect]
        public UserCreationForm ClickOk()
        {
            _userCreationFormElement.FindElement(OkButtonLocator).Click();
            return this;
        }

        // [LogAspect]
        public UserCreationForm ClickCancel()
        {
            _userCreationFormElement.FindElement(CancelButtonLocator).Click();
            return this;
        }

        // [LogAspect]
        public UserCreationForm ClickClose()
        {
            _userCreationFormElement.FindElement(CloseButtonLocator).Click();
            return this;
        }


        // [LogAspect]
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

        // [LogAspect]
        public UserCreationForm SetForcedPasswordChange(bool isForced)
        {
            var forcePasswordChangeCheckbox = _userCreationFormElement.FindElement(ForcePasswordChangeLocator);
            if (forcePasswordChangeCheckbox.Selected != isForced)
            {
                forcePasswordChangeCheckbox.Click();
            }

            return this;
        }

        // [LogAspect]
        public EditUserPage SubmitAndOpenEditUserPage()
        {
            ClickOk();
            return new EditUserPage(Browser);
        }

        // [LogAspect]
        public UsersPage Cancel()
        {
            ClickCancel();
            WaitHelpers.Wait(() => !_userCreationFormElement.IsElementDisplayed(UserCreationFormLocator));

            return _parentPage;
        }

        // [LogAspect]
        public UsersPage Close()
        {
            ClickClose();
            WaitHelpers.Wait(() => !_userCreationFormElement.IsElementDisplayed(UserCreationFormLocator));

            return _parentPage;
        }

        // [LogAspect]
        public string GetErrorBulbMessage()
        {
            var errorBulb = _userCreationFormElement.FindElementWithWait(ErrorBulbLocator);
            new Actions(Browser)
                .MoveToElement(errorBulb)
                .MoveByOffset(1, 1)
                .MoveByOffset(-1, -1)
                .Perform(); // move cursor to error bulb

            // Error bulb message is not part of user creation form
            return Browser.FindElementWithWait(ErrorBulbMessageLocator).Text;
        }
    }
}
