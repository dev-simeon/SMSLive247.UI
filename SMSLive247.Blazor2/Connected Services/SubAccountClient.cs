using System.Net.Http.Headers;

namespace SMSLive247.OpenApi
{
    public class SubAccountClient(HttpClient httpClient) : ApiClient(httpClient)
    {
        public SubAccountClient Impersonate(AccountResponse? account)
        {
            var apiKey = "fail-safely-by-sending-useless-api-key";
            try
            {
                apiKey = (new Guid(account!.Key!)).ToString();
            }
            finally  
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", apiKey);
            }
            return this;
        }
    }
}
