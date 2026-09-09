using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Headers;

namespace SMSLive247.OpenApi
{
    public partial class ApiClient
    {
        //private readonly AuthenticationStateProvider? _authProvider;

        //public ApiClient(HttpClient httpClient, AuthenticationStateProvider authProvider) : this(httpClient)
        //{
        //    _authProvider = authProvider;
        //}

        //partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        //{
        //    var member = _authProvider?.GetCachedMember();

        //    if (member != null)
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", member.ApiKey);
        //}



        partial void ProcessResponse(HttpClient client, HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Created)
                response.StatusCode = HttpStatusCode.OK;
        }
    }

    public partial class AccountResponse
    {
        public string Initials => $"{FirstName?.Trim().FirstOrDefault()}{LastName?.Trim().FirstOrDefault()}";
        public string FullName => $"{FirstName} {LastName}";
    }
    public partial class FileParameter
    {
        public FileParameter(IBrowserFile file)
            : this(file.OpenReadStream(), file.Name, file.ContentType) { }
    }

}
