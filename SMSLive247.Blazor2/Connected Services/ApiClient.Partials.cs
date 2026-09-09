using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SMSLive247.Authentication;

namespace SMSLive247.OpenApi
{
    public partial class ApiClient
    {
        private readonly SmsAuthProvider? _authProvider;

        public ApiClient(HttpClient client, AuthenticationStateProvider authProvider) : this(client)
        {
            _authProvider = (SmsAuthProvider)authProvider;
        }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            var apiKey = _authProvider?.GetApiKey();

            if (apiKey != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey.ToString());
        }

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

    public partial class ApiException
    {
        public string FriendlyErrorMessage
        {
            get
            {
                if (Response != null)
                {
                    try
                    {
                        var error = JsonSerializer.Deserialize<ApiError>(Response);
                        return error?.Message ?? Response;
                    }
                    catch (Exception ex)
                    {
                        return Response;
                    }
                }
                else if (!string.IsNullOrEmpty(Message))
                {
                    return Message;
                }
                else
                {
                    return "An unknown API error occurred.";
                }
            }
        }

        public record class ApiError()
        {
            [JsonPropertyName("code")] public int Code { get; set; }
            [JsonPropertyName("message")] public string Message { get; set; } = "";
            [JsonPropertyName("errors")] public FieldError[]? Errors { get; set; }

            public class FieldError
            {
                [JsonPropertyName("field")] public string Field { get; set; } = "";
                [JsonPropertyName("message")] public string Message { get; set; } = "";
            }
        }
    }
}
