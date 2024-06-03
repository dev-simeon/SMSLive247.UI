using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLive247.UI.Services
{
    public class AuthDelegateHandler(AuthenticationStateProvider auth) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKey = (await ((Authentication.SmsAuthProvider)auth).GetMember())?.ApiKey;

            if (apiKey != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            }

            return await base.SendAsync(request, cancellationToken);

            //var response = await base.SendAsync(request, cancellationToken);

            //if (response.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //    //if response is 401, redirect to '/Login'
            //    response = new HttpResponseMessage(HttpStatusCode.Redirect);
            //    response.Headers.Location = new Uri("/Login");
            //}

            //return response;
        }
    }
}
