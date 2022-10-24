using NUnit.Framework;
using YouTrackWebdriverTests.Model;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormTests.Fields
{
    public class FullNameTests : UsersCreationTestsBase
    {
        private const string OneSymbol = "1";
        private const string CorrectFullName = "Дмитрий Фалейчик";
        private const string MaxLengthString = "123456789.123456789.123456789.123456789.123456789.";
        private const string SpecialCharacters = @"{}|[]\^~!*""'(),$-_@.&=;:?#%";
        private const string UnicodeCharacters = "\uFF41\uFF45\uFF53\uFF54\uFF48\uFF45\uFF54\uFF49\uFF43\uFF53";

        [TestCase(OneSymbol, TestName = "One symbol password")]
        [TestCase(CorrectFullName, TestName = "Some correct password")]
        [TestCase(MaxLengthString, TestName = "Maximum length password")]
        [TestCase(SpecialCharacters, TestName = "Special characters in password")]
        [TestCase(UnicodeCharacters, TestName = "Unicode characters in password")]
        public void ValidFullName(string fullName)
        {
            // Given
            var user = UserCreator.CreateFilledUser(fullName: fullName);

            // When
            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(fullName, userRow.FullName);
            Assert.AreEqual(fullName, userRow.FullNameTitle);
        }

        [Test]
        public void FormDoesntAllowToTypeTooLongFullName()
        {
            // Given
            const string fullName = MaxLengthString + "moresymbols";

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm.TypeFullName(fullName);

            // Then
            Assert.AreEqual(MaxLengthString, userCreationForm.FullName);
        }
    }
}