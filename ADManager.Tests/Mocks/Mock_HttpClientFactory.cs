namespace ADManager.Tests.Mocks
{
    internal class Mock_HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            HttpClient client = new();
            return client;
        }
    }
}
