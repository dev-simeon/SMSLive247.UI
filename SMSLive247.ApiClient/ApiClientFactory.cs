namespace SMSLive247.OpenApi
{
    public class ApiClientFactory : ApiClient
    {
        public ApiClientFactory(Settings settings, HttpClient httpClient) : base(httpClient)
        {
            BaseUrl = settings.BaseUrl;
        }

        public record Settings(string BaseUrl);
    }

}
