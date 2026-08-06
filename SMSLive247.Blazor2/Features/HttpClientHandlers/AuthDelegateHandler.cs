using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLive247.UI.Services
{
    public class AuthDelegateHandler(AuthenticationStateProvider auth) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var apiKey = (await ((Authentication.SmsAuthProvider)auth).GetMember())?.ApiKey;

            var apiKey = "35F0F673-B828-4F31-AAF3-09ABB9285A2A";

            if (apiKey != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
