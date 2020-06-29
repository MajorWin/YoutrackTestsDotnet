using OpenQA.Selenium;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class YoutrackSettingsPage : PageObject
    {
        public const string AbsolutePath = "/setUp";

        private static readonly By LicenceCheckBoxLocator = By.Id("id_l.S.SetupContent.license.acceptLicenseAgreementLabel");
        private static readonly By PasswordTextBoxLocator = By.Id("id_l.S.SetupContent.rootPwd.rootPassword2");
        private static readonly By PasswordConfirmationTextBoxLocator = By.Id("id_l.S.SetupContent.rootPwd.confirmRootPassword");
        private static readonly By SaveButtonLocator = By.Id("id_l.S.SetupContent.saveButton");


        public YoutrackSettingsPage(IWebDriver browser) :
            base(browser, UriValidators.Equals(AbsolutePath, browser)) { }


        /// <summary>
        /// The only thing we need from this page in this project is to be able to login
        /// </summary>
        [LogAspect]
        public ProjectsPage InitialSetup(string rootLogin, string rootPassword)
        {
            FindElement(LicenceCheckBoxLocator).Click();
            FindElement(PasswordTextBoxLocator).SendKeys(rootPassword);
            FindElement(PasswordConfirmationTextBoxLocator).SendKeys(rootPassword);
            FindElement(SaveButtonLocator).Click();

            return new ProjectsPage(Browser);
        }
    }
}
