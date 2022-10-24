using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Model;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormTests.Fields
{
    public class PasswordTests : UsersCreationTestsBase
    {
        private const string OneSymbol = "1";
        private const string CorrectPassword = "tencharacters";
        private const string IncorrectPassword = "nottencharacters";

        private const string MaxLengthPassword = "123456789.123456789.123456789.123456789.123456789.";

        private const string TooLongPassword = "123456789.123456789.123456789.123456789.123456789.>";

        private const string UnicodeString = "\uFF41\uFF45\uFF53\uFF54\uFF48\uFF45\uFF54\uFF49\uFF43\uFF53";


        [TestCase(OneSymbol, TestName = "One symbol password")]
        [TestCase(CorrectPassword, TestName = "Correct password")]
        [TestCase(MaxLengthPassword, TestName = "Maximum length password")]
        [TestCase(UnicodeString, TestName = "Unicode characters in password")]
        public void ValidPasswordAndConfirmation(string password)
        {
            // Given
            var user = UserCreator.CreateFilledUser(password: password, passwordConfirmation: password);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            // Then
            using var anotherSession = CreateNewSession();
            Assert.DoesNotThrow(() => anotherSession.GoToLoginPage().LoginSuccessfully(user.Login, user.Password));
        }

        [TestCase(
            "",
            "",
            "Password is required!",
            TestName = "No password, no confirmation")]
        [TestCase(
            "",
            CorrectPassword,
            "Password doesn't match!",
            TestName = "No password")]
        [TestCase(
            CorrectPassword,
            "",
            "Password doesn't match!",
            TestName = "No confirmation")]
        [TestCase(
            CorrectPassword,
            IncorrectPassword,
            "Password doesn't match!",
            TestName = "Password and confirmation don't match")]
        [TestCase(
            MaxLengthPassword,
            MaxLengthPassword + "evenmore",
            "Password doesn't match!",
            TestName = "Confirmation is more than 50 characters")]
        public void NegativeTests(string password, string confirmation, string expectedErrorMessage)
        {
            // Given
            var user = UserCreator.CreateFilledUser(password: password, passwordConfirmation: confirmation);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorMessage = userCreationForm.GetErrorBulbMessage();
            Assert.AreEqual(expectedErrorMessage, errorMessage);

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorPopup());
        }

        [TestCase(
            TooLongPassword,
            "",
            TestName = "Password is > 50 characters long, password confirmation is empty")]
        [TestCase(
            TooLongPassword,
            CorrectPassword,
            TestName = "Password is > 50 characters long, password confirmation is of normal length")]
        [TestCase(
            TooLongPassword,
            TooLongPassword,
            TestName = "Password is > 50 characters long, password confirmation too")]
        public void TooLongPasswordReturnsError(string password, string passwordConfirmation)
        {
            // Given
            var user = UserCreator.CreateFilledUser(
                password: password,
                passwordConfirmation: passwordConfirmation);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .ClickOk();

            // Then
            var errorMessage = userCreationForm.GetErrorBulbMessage();
            Assert.AreEqual("Password shouldn't be more than 50 characters long", errorMessage);

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorPopup());
        }
    }
}