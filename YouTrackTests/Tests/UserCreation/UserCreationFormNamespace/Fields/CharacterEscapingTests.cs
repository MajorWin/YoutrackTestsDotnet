using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;
using YouTrackWebdriverTests.Validation;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormNamespace.Fields
{
    public class CharacterEscapingTests : UsersCreationTestsBase
    {
        private const string UrlSpecialCharacters = "?#%";
        private const string UrlSpecialCharactersEncoded = "%3F%23%25";

        [Test]
        public void UrlSpecialCharacteresAreEncoded()
        {
            // Given
            const string login = UrlSpecialCharacters;
            const string expectedLoginFromUri = UrlSpecialCharactersEncoded;

            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(login: login);

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm.Fill(user);
            var editUserPage = userCreationForm.SubmitSuccessfully();

            var urlEncodedLogin = editUserPage.LoginFromUri;

            // Then
            Assert.AreEqual(expectedLoginFromUri, urlEncodedLogin);

            using var anotherSession = CreateNewSession();
            Assert.DoesNotThrow(() => anotherSession.GoToLoginPage().LoginSuccessfully(user.Login, user.Password));
        }

        [TestCase(
            "some<login",
            "login shouldn't contain characters \"<\", \"/\", \">\": login"
            /*, TestName = "'<' in login"*/)]
        [TestCase(
            "some/login",
            "login shouldn't contain characters \"<\", \"/\", \">\": login"
            /*, TestName = "'/' in login"*/)]
        [TestCase(
            "some>login",
            "login shouldn't contain characters \"<\", \"/\", \">\": login"
            /*, TestName = "'>' in login"*/)]
        public void HtmlTagsInLogin(string login, string message)
        {
            // Given
            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(login: login);

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            CustomAssert.ContainsOnly(message, errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }

        [Test]
        public void HtmlTagsInAllFieldsExceptLogin()
        {
            // Given
            const string tagString = "\"'></body>";

            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(
                fullName: tagString,
                email: tagString,
                jabber: tagString);

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm.Fill(user);
            userCreationForm.SubmitSuccessfully();

            usersPage = GoToUsersPage();
            var userRow = usersPage.UserTable.FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(tagString, userRow.FullName);
            Assert.AreEqual(tagString, userRow.FullNameTitle);
            Assert.AreEqual(tagString, userRow.Email);
            Assert.AreEqual(tagString, userRow.EmailTitle);
            Assert.AreEqual(tagString, userRow.Jabber);
            Assert.AreEqual(tagString, userRow.JabberTitle);

            using var anotherSession = CreateNewSession();
            Assert.DoesNotThrow(() => anotherSession.GoToLoginPage().LoginSuccessfully(user.Login, user.Password));
        }

        [Test]
        public void HtmlSpecialCharactersInAllFields()
        {
            // Given
            const string specialCharacterString = "\"'&quot;&#39;&gt;&lt;&amp;";

            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(
                login: specialCharacterString,
                password: specialCharacterString,
                passwordConfirmation: specialCharacterString,
                fullName: specialCharacterString,
                email: specialCharacterString,
                jabber: specialCharacterString);

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm.Fill(user);
            userCreationForm.SubmitSuccessfully();

            usersPage = GoToUsersPage();
            var userRow = usersPage.UserTable.FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(specialCharacterString, userRow.Login);
            Assert.AreEqual(specialCharacterString, userRow.LoginTitle);
            Assert.AreEqual(specialCharacterString, userRow.FullName);
            Assert.AreEqual(specialCharacterString, userRow.FullNameTitle);
            Assert.AreEqual(specialCharacterString, userRow.Email);
            Assert.AreEqual(specialCharacterString, userRow.EmailTitle);
            Assert.AreEqual(specialCharacterString, userRow.Jabber);
            Assert.AreEqual(specialCharacterString, userRow.JabberTitle);

            using var anotherSession = CreateNewSession();
            Assert.DoesNotThrow(() => anotherSession.GoToLoginPage().LoginSuccessfully(user.Login, user.Password));
        }

        [Test]
        public void HtmlSpecialCharactersInDefaultFullname()
        {
            // Given
            const string specialCharacterString = "\"'&quot;&#39;&gt;&lt;&amp;";

            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(login: specialCharacterString, fullName: "");

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm.Fill(user);
            userCreationForm.SubmitSuccessfully();

            usersPage = GoToUsersPage();
            var userRow = usersPage.UserTable.FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(specialCharacterString, userRow.FullName);
            Assert.AreEqual(specialCharacterString, userRow.FullNameTitle);
        }
    }
}
