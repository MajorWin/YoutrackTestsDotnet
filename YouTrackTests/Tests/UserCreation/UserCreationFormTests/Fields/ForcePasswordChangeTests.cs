using NUnit.Framework;
using YouTrackWebdriverTests.Model;
using YouTrackWebdriverTests.PageObjects;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormTests.Fields
{
    public class ForcePasswordChangeTests : UsersCreationTestsBase
    {
        [Test]
        public void ForcePasswordChange()
        {
            // Given
            var user = UserCreator.CreateFilledUser(isPasswordChangeForced: true);

            var userCreationForm = GoToUsersPage().OpenUserCreationForm();

            // When
            userCreationForm
                .Fill(user)
                .SubmitAndOpenEditUserPage();

            using var anotherSession = CreateNewSession();
            UserPage userPage = null;

            // Then
            Assert.DoesNotThrow(
                () =>
                    userPage = anotherSession
                        .GoToLoginPage()
                        .LoginByForcedToChangePasswordUserSuccessfully(user.Login, user.Password)
            );
            Assert.DoesNotThrow(() => userPage.WaitForChangePasswordForm());
        }
    }
}