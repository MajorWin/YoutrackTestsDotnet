using NUnit.Framework;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.PageObjects;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.Tests.UserCreation.UserCreationFormNamespace.Fields
{
    public class ForcePasswordChangeTests : UsersCreationTestsBase
    {
        [Test]
        public void ForcePasswordChange()
        {
            // Given
            var usersPage = GoToUsersPage();
            var user = UserCreationForm.User.CreateFilledUser(isPasswordChangeForced: true);

            // When
            var userCreationForm = usersPage.OpenUserCreationForm();
            userCreationForm
                .Fill(user)
                .SubmitSuccessfully();

            using var anotherSession = CreateNewSession();
            UserPage userPage = null;

            // Then
            Assert.DoesNotThrow(
                () =>
                    userPage = anotherSession
                        .GoToLoginPage()
                        .LoginByForcedToChangePasswordUserSuccessfully(user.Login, user.Password)
            );
            Assert.True(userPage.IsChangePasswordFormDisplayed());
        }
    }
}