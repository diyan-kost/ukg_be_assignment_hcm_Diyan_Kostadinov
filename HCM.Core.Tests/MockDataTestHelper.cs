using HCM.Infrastructure.Entities;

namespace HCM.Core.Tests
{
    public static class MockDataTestHelper
    {
        public static IEnumerable<object[]> InvalidUsernames = new List<object[]>
        {
            new object[] { "" },
            new object[] { "1" },
            new object[] { "12" },
            new object[] { "1a " },
            new object[] { new string('T', 101) },

        };


        public static IEnumerable<object[]> InvalidPasswords = new List<object[]>
        {
            new object[] { "" },
            new object[] { "123" },
            new object[] { "test" },
            new object[] { "TEST" },
            new object[] { "Te1 St" },
            new object[] { new string(' ', 50) },

        };

        public static IEnumerable<object[]> InvalidNames = new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { new string('T', 256) },

        };

        public static IEnumerable<object[]> InvalidAddresses = new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { new string('T', 256) },

        };

        public static IEnumerable<object[]> InvalidNationalIds = new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { new string('1', 256) },

        };
    }
}
