using System.Net;

namespace SMSLive247.OpenApi
{
    public partial class ApiClient
    {
        /// <summary>
        /// Hook called by generated NSwag client methods before response evaluation.
        /// Converts HTTP 201 Created to HTTP 200 OK so generated methods continue without throwing status mismatch errors.
        /// </summary>
        partial void ProcessResponse(HttpClient client, HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Created)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
        }
    }
}
