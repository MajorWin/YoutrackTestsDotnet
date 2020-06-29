using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;

namespace YouTrackWebdriverTests.Tests.UserCreation
{
    public class UserCreationTests : UsersCreationTestsBase
    {
        [Test]
        public void CheckTotalUsersIncludingRootAndGuest()
        {
            // Given
            const int maxUsers = 9;
            string[] totalUsersExpected =
            {
                "(2 total)", // root and banned guest
                "(3 total)",
                "(4 total)",
                "(5 total)",
                "(6 total)",
                "(7 total)",
                "(8 total)",
                "(9 total)",
                "(10 total)",
                "(11 total)" // root, banned guest and 9 new users
            };

            var users = Enumerable.Range(1, maxUsers)
                .Select(_ => UserCreationForm.User.CreateFilledUser());

            // When
            var usersPage = GoToUsersPage();
            var totalUsersActual = new List<string>() {usersPage.UserTable.TotalUsers};

            foreach (var user in users)
            {
                usersPage
                    .OpenUserCreationForm()
                    .Fill(user)
                    .SubmitSuccessfully();

                usersPage = GoToUsersPage();
                totalUsersActual.Add(usersPage.UserTable.TotalUsers);
            }

            // Then
            CollectionAssert.AreEqual(totalUsersExpected, totalUsersActual);
        }
    }
}
