using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormNamespace
{
    public class UserCreationFormTests : UsersCreationTestsBase
    {
        [Test]
        public void CancelButtonClosesForm()
        {
            // Given
            var usersPage = GoToUsersPage();
            var userCreationForm = usersPage.OpenUserCreationForm();

            // When
            userCreationForm.ClickCancel();

            // Then
            Assert.True(WaitHelpers.Wait(IsFormClosed));
        }

        [Test]
        public void CloseButtonClosesForm()
        {
            // Given
            var usersPage = GoToUsersPage();
            var userCreationForm = usersPage.OpenUserCreationForm();

            // When
            userCreationForm.ClickClose();

            // Then
            Assert.True(WaitHelpers.Wait(IsFormClosed));
        }

        [Test]
        public void ReopenedFormIsEmpty()
        {
            // Given
            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(isPasswordChangeForced: true);

            // When
            usersPage
                .OpenUserCreationForm()
                .Fill(user)
                .Cancel();

            var userCreationForm = usersPage.OpenUserCreationForm();

            // Then
            Assert.IsEmpty(userCreationForm.Login);
            Assert.IsEmpty(userCreationForm.Password);
            Assert.IsEmpty(userCreationForm.PasswordConfirmation);
            Assert.False(userCreationForm.IsPasswordChangeForced);
            Assert.IsEmpty(userCreationForm.FullName);
            Assert.IsEmpty(userCreationForm.Email);
            Assert.IsEmpty(userCreationForm.Jabber);
        }

        [Test]
        public void CanSubmitFromReopenedForm()
        {
            // Given
            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser();

            // When
            usersPage = usersPage.OpenUserCreationForm().Close();

            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm
                .Fill(user)
                .SubmitSuccessfully();

            var userRow = GoToUsersPage().UserTable.FindRowByLogin(user.Login);

            // Then
            Assert.AreEqual(user.Login, userRow.Login);
            Assert.AreEqual(user.FullName, userRow.FullName);
            Assert.AreEqual(user.Email, userRow.Email);
            Assert.AreEqual(user.Jabber, userRow.Jabber);

            using var anotherSession = CreateNewSession();
            Assert.DoesNotThrow(() => anotherSession.GoToLoginPage().LoginSuccessfully(user.Login, user.Password));
        }

        // TODO: after this bug is fixed remove ErrorPopup.NonEmptyErrorMessages and use ErrorPopup.ErrorMessages
        [Test]
        public void ErrorPopupListContainsSingleMessage()
        {
            // Given
            var userWithIncorrectLogin = UserCreationForm.User.CreateFilledUser(login: "     ");

            // When
            var userCreationForm = GoToUsersPage().OpenUserCreationForm();
            userCreationForm
                .Fill(userWithIncorrectLogin)
                .ClickOk();


            // Then
            var errorPopup = userCreationForm.GetErrorPopup();
            Assert.AreEqual(1, errorPopup.ErrorMessages.Count());

            Assert.Throws<NoSuchElementException>(() => userCreationForm.GetErrorBulbMessage());
        }

        private static bool IsFormClosed() =>
            TestEnvironment.Browser.IsElementDisplayed(UserCreationForm.UserCreationFormLocator);
    }
}
