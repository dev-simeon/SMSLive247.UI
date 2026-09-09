using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLive247.UI.Services
{
    public class AuthDelegateHandler(AuthenticationStateProvider auth) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKey = ((Authentication.SmsAuthProvider)auth).GetApiKey();

            if (apiKey != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey.ToString());

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
