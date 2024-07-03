namespace SMSLive247.OpenApi
{
    public class ApiClientFactory(
        ApiClientFactory.ApiSettings settings,
        HttpClient httpClient) : ApiClient(settings.BaseUrl, httpClient)
    {
        public record class ApiSettings(string BaseUrl);
    }

}
