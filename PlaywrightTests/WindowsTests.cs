using Microsoft.Playwright;

namespace PlaywrightTests
{

    [TestFixture]
    public class WindowsTests : Tests
    {
        protected virtual string BaseUrl => Environment.GetEnvironmentVariable("AD_MANAGEMENT_TEST_URL") ?? "https://localhost";

    }
}
