using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace SMSLive247.OpenApi
{
    public partial class ApiClient
    {
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
