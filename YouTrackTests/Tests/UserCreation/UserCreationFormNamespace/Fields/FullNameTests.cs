using NUnit.Framework;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormNamespace.Fields
{
    public class FullNameTests : UsersCreationTestsBase
    {
        private const string OneSymbol = "1";
        private const string CorrectFullName = "Дмитрий Фалейчик";
        private const string MaxLengthString = "123456789.123456789.123456789.123456789.123456789.";
        private const string SpecialCharacters = @"{}|[]\^~!*""'(),$-_@.&=;:?#%";
        private const string UnicodeString = "\uFF41\uFF45\uFF53\uFF54\uFF48\uFF45\uFF54\uFF49\uFF43\uFF53";

        [TestCase(OneSymbol /*, TestName = "One symbol password"*/)]
        [TestCase(CorrectFullName /*, TestName = "Some correct password"*/)]
        [TestCase(MaxLengthString /*, TestName = "Maximum length password"*/)]
        [TestCase(SpecialCharacters /*, TestName = "Unicode symbols in password"*/)]
        [TestCase(UnicodeString /*, TestName = "Unicode symbols in password"*/)]
        public void ValidFullName(string fullName)
        {
            // Given
            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(fullName: fullName);

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm
                .Fill(user)
                .SubmitSuccessfully();

            usersPage = GoToUsersPage();
            var userRow = usersPage.UserTable.FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(fullName, userRow.FullName);
            Assert.AreEqual(fullName, userRow.FullNameTitle);
        }

        [Test]
        public void TooLongFullNameWillBeCutToMaxSize()
        {
            // Given
            const string fullName = MaxLengthString + "moresymbols";

            var usersPage = GoToUsersPage();

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm.TypeFullName(fullName);

            // Then
            Assert.AreEqual(MaxLengthString, userCreationForm.FullName);
        }
    }
}
