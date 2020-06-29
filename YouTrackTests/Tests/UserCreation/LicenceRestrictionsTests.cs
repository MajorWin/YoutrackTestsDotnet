using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;
using YouTrackWebdriverTests.Validation;


namespace YouTrackWebdriverTests.Tests.UserCreation
{
    public class LicenceRestrictionsTests : UsersCreationTestsBase
    {
        private const int ActiveUsersAllowed = 10;

        [Test]
        public void CreateUserLinkDissappearsAsUserLimitReached()
        {
            // When
            CreateMaximumActiveUsers();

            // Then
            Assert.Throws<NoSuchElementException>(() => GoToUsersPage().ClickCreateUser());
        }

        [Test]
        public void CantCreateUserFromPageOpenedBeforeUserLimitReaches()
        {
            // Given
            using var anotherSession = CreateNewSession();
            anotherSession.GoToLoginPage()
                .LoginSuccessfully(TestEnvironment.RootLogin, TestEnvironment.Password);

            var usersPageFromAnotherSession = anotherSession.GoToUsersPage();

            // When
            CreateMaximumActiveUsers();

            usersPageFromAnotherSession.ClickCreateUser();

            // Then
            var errorPopup = usersPageFromAnotherSession.GetErrorPopup();
            CustomAssert.ContainsOnly("Can't create user.", errorPopup.NonEmptyErrorMessages.ToList());
        }

        [Test]
        public void CantCreateUserFromTheFormOpenedBeforeUserLimitReaches()
        {
            // Given
            using var anotherSession = CreateNewSession();
            anotherSession.GoToLoginPage()
                .LoginSuccessfully(TestEnvironment.RootLogin, TestEnvironment.Password);

            var userCreationFormFromAnotherSession = anotherSession.GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(UserCreationForm.User.CreateFilledUser());

            // When
            CreateMaximumActiveUsers();

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
            var user = UserCreationForm.User.CreateFilledUser();
            var user2 = UserCreationForm.User.CreateFilledUser();

            // When
            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(user)
                .SubmitSuccessfully();

            CreateMaximumActiveUsers();

            GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login)
                .DeleteUser();

            // Then
            Assert.DoesNotThrow(
                () => GoToUsersPage()
                    .OpenUserCreationForm()
                    .Fill(user2)
                    .SubmitSuccessfully());
        }

        [Test]
        public void CanCreateNewUserAfterFreeingUserSlotByBanningOne()
        {
            // Given
            var user = UserCreationForm.User.CreateFilledUser();
            var user2 = UserCreationForm.User.CreateFilledUser();

            // When
            GoToUsersPage()
                .OpenUserCreationForm()
                .Fill(user)
                .SubmitSuccessfully();

            CreateMaximumActiveUsers();

            GoToUsersPage()
                .UserTable
                .FindRowByLogin(user.Login)
                .BanUser();

            // Then
            Assert.DoesNotThrow(
                () => GoToUsersPage()
                    .OpenUserCreationForm()
                    .Fill(user2)
                    .SubmitSuccessfully());
        }


        private static int CountActiveUsers()
        {
            var userRows = GoToUsersPage()
                .UserTable
                .UserRows
                .ToList();
            return userRows.Count - userRows.Count(userRow => userRow.IsBanned);
        }

        private static void CreateMaximumActiveUsers()
        {
            var userSlotsRemaining = ActiveUsersAllowed - CountActiveUsers();

            for (var i = 0; i < userSlotsRemaining; i++)
            {
                var user = UserCreationForm.User.CreateFilledUser();

                GoToUsersPage()
                    .OpenUserCreationForm()
                    .Fill(user)
                    .SubmitSuccessfully();
            }
        }
    }
}
