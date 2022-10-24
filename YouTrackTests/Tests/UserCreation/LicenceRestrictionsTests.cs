using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Model;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;
using YouTrackWebdriverTests.Validation;


namespace YouTrackWebdriverTests.Tests.UserCreation
{
    public class LicenceRestrictionsTests : UsersCreationTestsBase
    {
        [Test]
        public void CreateUserLinkDisappearsAsUserLimitReached()
        {
            // When
            YoutrackHelper.CreateMaximumActiveUsers();

            // Then
            Assert.Throws<NoSuchElementException>(() => GoToUsersPage().ClickCreateUser());
        }

        [Test]
        public void CantOpenUserCreationFormFromPageOpenedBeforeUserLimitReached()
        {
            // Given
            using var anotherSession = CreateNewSession();
            anotherSession.GoToLoginPage()
                .LoginSuccessfully(Configuration.Login, Configuration.Password);

            var usersPageFromAnotherSession = anotherSession.GoToUsersPage();

            // When
            YoutrackHelper.CreateMaximumActiveUsers();

            usersPageFromAnotherSession.ClickCreateUser();

            // Then
            var errorPopup = usersPageFromAnotherSession.GetErrorPopup();
            CustomAssert.ContainsOnly("Can't create user.", errorPopup.NonEmptyErrorMessages.ToList());
        }

        [Test]
        public void CantCreateUserFromTheFormOpenedBeforeUserLimitReached()
        {
            // Given
            using var anotherSession = CreateNewSession();
            anotherSession.GoToLoginPage()
                .LoginSuccessfully(Configuration.Login, Configuration.Password);

            var userCreationFormFromAnotherSession = anotherSession.GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(UserCreator.CreateFilledUser());

            // When
            YoutrackHelper.CreateMaximumActiveUsers();

            userCreationFormFromAnotherSession.ClickOk();

            // Then
            var errorPopup = userCreationFormFromAnotherSession.GetErrorPopup();
            CustomAssert.ContainsOnly(
                "Can't create user because of license restrictions!",
                errorPopup.NonEmptyErrorMessages.ToList());

            Assert.Throws<NoSuchElementException>(() => userCreationFormFromAnotherSession.GetErrorBulbMessage());
        }

        [Test]
        public void CanCreateNewUserAfterFreeingUserSlotByDeletingOne()
        {
            // Given
            var userToDelete = UserCreator.CreateFilledUser();
            var userToCreate = UserCreator.CreateFilledUser();

            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(userToDelete)
                .SubmitAndOpenEditUserPage();

            // When
            YoutrackHelper.CreateMaximumActiveUsers();

            GoToUsersPage()
                .UserTable
                .FindRowByLogin(userToDelete.Login)
                .DeleteUser();

            // Then
            Assert.DoesNotThrow(
                () => GoToUsersPage()
                    .OpenUserCreationForm()
                    .Fill(userToCreate)
                    .SubmitAndOpenEditUserPage());
        }

        [Test]
        public void CanCreateNewUserAfterFreeingUserSlotByBanningOne()
        {
            // Given
            var userToBan = UserCreator.CreateFilledUser();
            var userToCreate = UserCreator.CreateFilledUser();

            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(userToBan)
                .SubmitAndOpenEditUserPage();

            // When
            YoutrackHelper.CreateMaximumActiveUsers();

            GoToUsersPage()
                .UserTable
                .FindRowByLogin(userToBan.Login)
                .BanUser();

            // Then
            Assert.DoesNotThrow(
                () => GoToUsersPage()
                    .OpenUserCreationForm()
                    .Fill(userToCreate)
                    .SubmitAndOpenEditUserPage());
        }
    }
}
