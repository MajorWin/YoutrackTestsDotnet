using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Model;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;
using YouTrackWebdriverTests.Validation;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormTests.Fields
{
    public class LoginTests : UsersCreationTestsBase
    {
        private const string OneSymbol = "1";
        private const string TenSymbols = "tensymbols";
        private const string MaxLengthLogin = "123456789.123456789.123456789.123456789.123456789.";
        private const string SpecialCharacters = @"{}|[]\^~!*""'(),$-_@.&=;:?#%";
        private const string UnicodeString = "\uFF41\uFF45\uFF53\uFF54\uFF48\uFF45\uFF54\uFF49\uFF43\uFF53";

        private const string StringWithPlus = "a+b";
        private const string StringWithSpace = "a b";

        private const string ZeroWidthSpace = "\u200B"; // invisible character

        // More: https://en.wikipedia.org/wiki/Whitespace_character
        private static readonly IEnumerable<string> SomeUnicodeWhitespaces = new[]
        {
            "\u00A0", "\u2000", "\u200A", "\u3000"
        };

        private static readonly IEnumerable<string> StringsWithUnicodeWhitespaces = SomeUnicodeWhitespaces
            .Select(whitespace => $"login{whitespace}withwhitespace");


        [TestCase(
            OneSymbol,
            OneSymbol,
            OneSymbol,
            TestName = "One symbol")]
        [TestCase(
            TenSymbols,
            TenSymbols,
            TenSymbols,
            TestName = "Ten symbols")]
        [TestCase(
            MaxLengthLogin,
            MaxLengthLogin,
            MaxLengthLogin,
            TestName = "Max length")]
        [TestCase(
            SpecialCharacters,
            SpecialCharacters,
            SpecialCharacters,
            TestName = "Special characters")]
        [TestCase(
            StringWithPlus,
            StringWithSpace,
            StringWithPlus,
            TestName = "Plus symbol is not url encoded")]
        [TestCase(
            UnicodeString,
            UnicodeString,
            UnicodeString,
            TestName = "Unicode characters in login")]
        public void ValidLoginEmptyFullName(string login, string expectedLoginFromEditUserPageUri,
            string expectedLoginFromUserTable)
        {
            // Given
            var user = UserCreator.CreateFilledUser(login: login, fullName: "");

            // When
            var editUserPage = GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(user)
                .SubmitAndOpenEditUserPage();
            var editUserPageUri = editUserPage.GetUri();
            var loginFromUri = editUserPage.GetLoginFromUri();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(expectedLoginFromUserTable);

            // Then
            Assert.AreEqual(expectedLoginFromEditUserPageUri, loginFromUri);

            Assert.AreEqual(expectedLoginFromUserTable, userRow.Login);
            Assert.AreEqual(expectedLoginFromUserTable, userRow.LoginTitle);

            Assert.AreEqual(expectedLoginFromUserTable, userRow.FullName);
            Assert.AreEqual(expectedLoginFromUserTable, userRow.FullNameTitle);

            Assert.AreEqual(editUserPageUri, userRow.LoginLink);

            using var anotherSession = CreateNewSession();
            Assert.DoesNotThrow(() => anotherSession.GoToLoginPage().LoginSuccessfully(user.Login, user.Password));
        }

        [Test]
        public void FormDoesntAllowToTypeTooLongLogin()
        {
            // Given
            const string login = MaxLengthLogin + ">";

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm.TypeLogin(login);

            // Then
            Assert.AreEqual(MaxLengthLogin, userCreationForm.Login);
        }

        [TestCase("", "Login is required!", TestName = "Empty login")]
        [TestCase("      ", "Login is required!", TestName = "Login consists of spaces")]
        public void ErrorBulbErrors(string login, string message)
        {
            // Given
            var user = UserCreator.CreateFilledUser(login: login);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorBulbMessage = userCreationForm.GetErrorBulbMessage();
            Assert.AreEqual(message, errorBulbMessage);

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorPopup());
        }

        [TestCase(
            " a",
            "Restricted character ' ' in the name",
            TestName = "Space at the start of login")]
        [TestCase(
            "a ",
            "Restricted character ' ' in the name",
            TestName = "Space at the end of login")]
        [TestCase(
            "a a",
            "Restricted character ' ' in the name",
            TestName = "Space in the middle of login")]
        [TestCase(
            ".",
            "Can't use \"..\", \".\" for login: login",
            TestName = "Login is '.'")]
        [TestCase(
            "..",
            "Can't use \"..\", \".\" for login: login",
            TestName = "Login is '..'")]
        public void ErrorPopupErrors(string login, string message)
        {
            // Given
            var user = UserCreator.CreateFilledUser(login: login);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            CustomAssert.ContainsOnly(message, errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }

        [TestCase("user1", "user1", TestName = "Same logins")]
        [TestCase("user1", "USER1", TestName = "First login is lowercased, second is uppercased")]
        [TestCase("USER1", "user1", TestName = "First login is uppercased, second is lowercased")]
        public void NotAllowedToCreateUserWithNonUniqueLogin(string login1, string login2)
        {
            // Given
            var user1 = UserCreator.CreateFilledUser(login: login1);
            var user2 = UserCreator.CreateFilledUser(login: login2);

            // When
            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(user1)
                .SubmitAndOpenEditUserPage();

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();
            userCreationForm
                .Fill(user2)
                .ClickOk();

            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            CustomAssert.ContainsOnly("Value should be unique: login", errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }

        [TestCase("root")]
        [TestCase("guest")]
        public void NotAllowedToCreateUserWithRootAndGuestNames(string login)
        {
            // Given
            var user = UserCreator.CreateFilledUser(login: login);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            CustomAssert.ContainsOnly("Value should be unique: login", errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }

        [Test]
        public void NotAllowedToCreateUsersWithIdenticalLookingLogin()
        {
            // Given
            const string login1 = "login_with_english_e";
            const string login2 = "login_with_russian_е";

            var user1 = UserCreator.CreateFilledUser(login: login1);
            var user2 = UserCreator.CreateFilledUser(login: login2);

            // When
            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(user1)
                .SubmitAndOpenEditUserPage();

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();
            userCreationForm
                .Fill(user2)
                .ClickOk();

            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            CustomAssert.ContainsOnly("Clarify error message", errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }


        [TestCaseSource(nameof(SomeUnicodeWhitespaces))]
        [TestCaseSource(nameof(StringsWithUnicodeWhitespaces))]
        // Can not delete user after next test, because WebDriver returns it's name as "root" and not "root<invis char>"
        // [TestCase("root" + ZeroWidthSpace, TestName = "'root' with zerowidth whitespace at the end")]
        public void WhitespacesNotAllowedInLogin(string login)
        {
            // Given
            var usersPage = GoToUsersPage();
            var user = UserCreator.CreateFilledUser(login: login);

            var userCreationForm = usersPage.OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            CustomAssert.ContainsOnly("Clarify error message", errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }
    }
}