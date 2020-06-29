using NUnit.Framework;

namespace YouTrackWebdriverTests.Tests
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            YoutrackHelper.InitialSetupAndLoginAsRoot(TestEnvironment.Browser);
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            TestEnvironment.Browser.Dispose();
        }
    }
}
