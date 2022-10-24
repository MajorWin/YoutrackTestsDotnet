using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class YoutrackSettingsPage : YoutrackPageObject
    {
        public const string Path = "/setUp";

        private static readonly By LicenceCheckBoxLocator = By.Id("id_l.S.SetupContent.license.acceptLicenseAgreementLabel");
        private static readonly By PasswordTextBoxLocator = By.Id("id_l.S.SetupContent.rootPwd.rootPassword2");
        private static readonly By PasswordConfirmationTextBoxLocator = By.Id("id_l.S.SetupContent.rootPwd.confirmRootPassword");
        private static readonly By SaveButtonLocator = By.Id("id_l.S.SetupContent.saveButton");


        public YoutrackSettingsPage(IWebDriver browser) :
            base(browser, new UriPathMatchesValidator(browser, Path))
        {
        }


        /// <summary>
        /// The only thing we need from this page in this project is to be able to login
        /// </summary>
        // [LogAspect]
        public ProjectsPage InitialSetup(string rootLogin, string rootPassword)
        {
            Browser.FindElement(LicenceCheckBoxLocator).Click();
            Browser.FindElement(PasswordTextBoxLocator).SendKeys(rootPassword);
            Browser.FindElement(PasswordConfirmationTextBoxLocator).SendKeys(rootPassword);
            Browser.FindElement(SaveButtonLocator).Click();

            return new ProjectsPage(Browser);
        }
    }
}
