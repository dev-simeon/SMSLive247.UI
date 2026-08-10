using Microsoft.AspNetCore.Components.Authorization;
using SMSLive247.UI.Services;
using SMSLive247.UI.Shared;
using SMSLive247.OpenApi;
using SMSLive247.Authentication;
using SMSLive247.Blazor2.Components;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var settings = new Settings();
        builder.Configuration.Bind(settings);
        builder.Services.AddSingleton(settings);

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication("Cookies")
            .AddCookie(options => { options.LoginPath = "/"; });

        builder.Services.AddSingleton<AlertService>();
        builder.Services.AddTransient<AuthDelegateHandler>();
        //builder.Services.AddTransient<CacheDelegateHandler>();

        builder.Services.AddHttpClient<SubAccountClient>(ConfigureUrl);
        builder.Services.AddHttpClient<ApiClient>(ConfigureUrl)
                        .AddHttpMessageHandler<AuthDelegateHandler>();

        builder.Services.AddScoped<AuthenticationStateProvider, SmsAuthProvider>();
        builder.Services.AddMemoryCache();

        // ── Middleware ───────────────────────────────────────────────────────
        var app = builder.Build();

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();
        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();

        void ConfigureUrl(HttpClient client)
        {
            client.BaseAddress = new Uri(settings.BaseUrl);
        }
    }
}