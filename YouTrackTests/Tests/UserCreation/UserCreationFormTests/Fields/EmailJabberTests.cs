using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Model;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormTests.Fields
{
    public class EmailJabberTests : UsersCreationTestsBase
    {
        private const string OneSymbol = "1";
        private const string CorrectEmail = "fefe@mail.hot-chilli.net";
        private const string CorrectJabber = "username@jabber.org";
        private const string MaxLengthString = "123456789.123456789.123456789.123456789.123456789.";
        private const string SpecialCharacters = @"{}|[]\^~!*""'(),$-_@.&=;:?#%";
        private const string UnicodeCharacters = "\uFF41\uFF45\uFF53\uFF54\uFF48\uFF45\uFF54\uFF49\uFF43\uFF53";


        [TestCase(OneSymbol, TestName = "One symbol")]
        [TestCase(CorrectEmail, TestName = "Correct email")]
        [TestCase(MaxLengthString, TestName = "Maximum length email")]
        [TestCase(SpecialCharacters, TestName = "Special characters in email")]
        [TestCase(UnicodeCharacters, TestName = "Unicode characters in email")]
        public void ValidEmail(string email)
        {
            // Given
            var user = UserCreator.CreateFilledUser(email: email);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(email, userRow.Email);
            Assert.AreEqual(email, userRow.EmailTitle);
        }

        [Test]
        public void TooLongEmail()
        {
            // Given
            const string email = MaxLengthString + "moresymbols";

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm.TypeEmail(email);

            // Then
            Assert.AreEqual(MaxLengthString, userCreationForm.Email);
        }

        [TestCase(OneSymbol, TestName = "One symbol jabber")]
        [TestCase(CorrectJabber, TestName = "Some correct jabber")]
        [TestCase(MaxLengthString, TestName = "Maximum length jabber")]
        [TestCase(SpecialCharacters, TestName = "Special characters in jabber")]
        [TestCase(UnicodeCharacters, TestName = "Unicode characters in jabber")]
        public void ValidJabber(string jabber)
        {
            // Given
            var user = UserCreator.CreateFilledUser(jabber: jabber);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(jabber, userRow.Jabber);
            Assert.AreEqual(jabber, userRow.JabberTitle);
        }

        [Test]
        public void TooLongJabber()
        {
            // Given
            const string jabber = MaxLengthString + "moresymbols";

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm.TypeJabber(jabber);

            // Then
            Assert.AreEqual(MaxLengthString, userCreationForm.Jabber);
        }

        [Test]
        public void EmailWithoutJabber()
        {
            // Given
            var user = UserCreator.CreateFilledUser(jabber: "");

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(user.Email, userRow.Email);
            Assert.AreEqual(user.Email, userRow.EmailTitle);
            Assert.Throws<NoSuchElementException>(() => userRow.Jabber.ToString());
        }

        [Test]
        public void JabberWithoutEmail()
        {
            // Given
            var user = UserCreator.CreateFilledUser(email: "");

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(user.Email, "");
            Assert.AreEqual(user.Email, "");
            Assert.AreEqual(user.Jabber, userRow.Jabber);
            Assert.AreEqual(user.Jabber, userRow.JabberTitle);
        }

        [Test]
        public void NoEmailNoJabber()
        {
            // Given
            var user = UserCreator.CreateFilledUser(email: "", jabber: "");

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            var userRow = GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(user.Email, "");
            Assert.AreEqual(user.Email, "");
            Assert.Throws<NoSuchElementException>(() => userRow.Jabber.ToString());
        }
    }
}