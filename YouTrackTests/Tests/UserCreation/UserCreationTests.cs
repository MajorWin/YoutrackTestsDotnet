using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YouTrackWebdriverTests.Model;

namespace YouTrackWebdriverTests.Tests.UserCreation
{
    public class UserCreationTests : UsersCreationTestsBase
    {
        private const int TotalUserDefault = 2; //root and banned guest
        private const int MaxNewUsers = 9;      // up to 10 active users for unlicensed version
        private const string TotalUsersTemplate = "({0} total)";

        [Test]
        public void CreateUpToLimitUsers()
        {
            // Given
            var totalUsersExpected = Enumerable.Range(TotalUserDefault, MaxNewUsers + 1)
                .Select(GetTotalUsersText);

            var users = Enumerable.Range(1, MaxNewUsers)
                .Select(_ => UserCreator.CreateFilledUser());

            // When
            var usersPage = GoToUsersPage();
            var totalUsersActual = new List<string> { usersPage.UserTable.TotalUsers };

            foreach (var user in users)
            {
                usersPage
                    .OpenUserCreationForm()
                    .Fill(user)
                    .SubmitAndOpenEditUserPage();

                usersPage = GoToUsersPage();
                totalUsersActual.Add(usersPage.UserTable.TotalUsers);
            }

            // Then
            CollectionAssert.AreEqual(totalUsersExpected, totalUsersActual);
        }


        private static string GetTotalUsersText(int usersCount) => string.Format(TotalUsersTemplate, usersCount);
    }
}