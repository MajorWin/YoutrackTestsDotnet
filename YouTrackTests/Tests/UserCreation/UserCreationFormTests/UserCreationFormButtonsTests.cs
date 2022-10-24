using NUnit.Framework;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormTests
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
            Assert.True(WaitHelpers.Wait(usersPage.IsFormClosed));
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
            Assert.True(WaitHelpers.Wait(usersPage.IsFormClosed));
        }
    }
}
