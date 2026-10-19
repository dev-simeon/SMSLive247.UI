using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SMSLive247.OpenApi;

public sealed class DedicatedAccountResponse
{
    [JsonPropertyName("accountNumber")] public string AccountNumber { get; set; } = "";
    [JsonPropertyName("accountName")] public string AccountName { get; set; } = "";
    [JsonPropertyName("bankName")] public string BankName { get; set; } = "";
    [JsonPropertyName("currency")] public string? Currency { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
}

public partial class ApiClient
{
    public async Task<DedicatedAccountResponse> DedicatedAccountEnsureAsync(CancellationToken cancellationToken = default)
    {
        using var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/v5/credits/dedicated-account/ensure");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await EnsureSuccess(response);
        return await response.Content.ReadFromJsonAsync<DedicatedAccountResponse>(JsonSerializerSettings, cancellationToken)
            ?? throw new InvalidOperationException("The dedicated account response was empty.");
    }

    private HttpRequestMessage CreateAuthenticatedRequest(HttpMethod method, string path)
    {
        var request = new HttpRequestMessage(method, path);
        var apiKey = _authProvider?.GetApiKey();
        if (apiKey != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey.ToString());
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return request;
    }

    private static async Task EnsureSuccess(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadAsStringAsync();
        var headers = response.Headers
            .Concat(response.Content.Headers)
            .GroupBy(header => header.Key)
            .ToDictionary(group => group.Key, group => (IEnumerable<string>)group.SelectMany(header => header.Value).ToArray());

        throw new ApiException("The HTTP status code of the response was not expected.",
            (int)response.StatusCode, body, headers, null);
    }
}
