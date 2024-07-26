using System.Net.Http.Headers;

namespace SMSLive247.OpenApi
{
    public class SubAccountClient(HttpClient httpClient) : ApiClient(httpClient)
    {
        public SubAccountClient Impersonate(AccountResponse account)
        {
            try
            {
                var apiKey = new Guid(account.Key!);

                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", apiKey.ToString());
                return this;
            }
            catch (Exception)
            {
                throw new Exception("Invalid Sub-Account API Key");
            }
        }
    }
}
