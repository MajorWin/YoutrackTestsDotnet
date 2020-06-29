using System.Collections;
using NUnit.Framework;

namespace YouTrackWebdriverTests.Validation
{
    public static class CustomAssert
    {
        public static void ContainsOnly(object expected, ICollection actual)
        {
            Assert.AreEqual(1, actual.Count);
            Assert.Contains(expected, actual);
        }
    }
}
