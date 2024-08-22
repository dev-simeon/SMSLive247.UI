using SMSLive247.OpenApi;
using SMSLive247.UI.Services;
using SMSLive247.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

namespace SMSLive247.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {       
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var settings = new Settings();
            builder.Configuration.Bind(settings);
            builder.Services.AddSingleton(settings);

            builder.Services.AddSingleton<AlertService>();
            builder.Services.AddSingleton<SpinnerService>();
            builder.Services.AddTransient<StorageService>();

            builder.Services.AddTransient<AuthDelegateHandler>();
            builder.Services.AddTransient<CacheDelegateHandler>();
            builder.Services.AddTransient<SpinnerDelegateHandler>();


            builder.Services.AddHttpClient<ApiClient>(ConfigureUrl)
                   .AddHttpMessageHandler<AuthDelegateHandler>();
                //.AddHttpMessageHandler<CacheDelegateHandler>();
                //.AddHttpMessageHandler<SpinnerDelegateHandler>();

            builder.Services.AddHttpClient<SubAccountClient>(ConfigureUrl);
                   //.AddHttpMessageHandler<SpinnerDelegateHandler>();

            builder.Services.AddMemoryCache();
            builder.Services.AddSmsAuthProvider();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();

            void ConfigureUrl(HttpClient client)
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
            }
        }
    }
}
