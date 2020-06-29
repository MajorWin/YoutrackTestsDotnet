using NUnit.Framework;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.Tests.UserCreation
{
    public abstract class UsersCreationTestsBase : TestBase
    {
        [SetUp]
        [LogAspect]
        public void Setup() { }

        [TearDown]
        [LogAspect]
        public void Teardown()
        {
            GoToUsersPage().UserTable.RemoveAllUsersExceptRootAndGuest();
        }

        protected static UsersPage GoToUsersPage() => TestEnvironment.Browser.GoToUsersPage();
    }
}
