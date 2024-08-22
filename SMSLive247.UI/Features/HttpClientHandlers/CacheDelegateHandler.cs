using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace SMSLive247.UI.Services
{
    public class CacheDelegateHandler(IMemoryCache cache) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method != HttpMethod.Get)
                return await base.SendAsync(request, cancellationToken);

            string key = request.RequestUri!.PathAndQuery;

            if (cache.TryGetValue(key, out HttpResponseMessage? response))
                return response;

            response = await base.SendAsync(request, cancellationToken);
          
            await cache.TrySet(key, response, cancellationToken);     
            return response;
        }    
    }

    public static class CacheExtensions
    {
        public static async Task TrySet(this IMemoryCache cache, string key, 
            HttpResponseMessage response, CancellationToken cancellationToken)  
        {
            if (!response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!string.IsNullOrEmpty(content))
            {
                var headers = response.Headers.AsString();
                cache.Set(key, $"{headers}\n\n\n{content}", TimeSpan.FromMinutes(3));
            }
        }

        public static bool TryGetValue(this IMemoryCache cache, string key, 
            [NotNullWhen(true)] out HttpResponseMessage? response)
        {
            if (cache.TryGetValue(key, out string? value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var array = value.Split("\n\n\n");
                    response = new HttpResponseMessage()
                    {
                        Content = new StringContent(array[1]),
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                    response.Headers.LoadFromString(array[0]);
                    return true;
                }
            }
            response = null;
            return false;
        }

        public static string AsString(this HttpResponseHeaders headers)
        {
            return headers.ToString();
        }

        public static void LoadFromString(this HttpResponseHeaders headers, string value)
        {
            var array = value.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            foreach(var item in array)
            {
                var a = item.Split(':');
                headers.Add(a[0], a[1]);
            }
        }
    }
}
