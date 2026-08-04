using SMSLive247.Blazor2.Components;
using SMSLive247.OpenApi;
using SMSLive247.UI;
using SMSLive247.UI.Services;

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

        builder.Services.AddHttpClient<ApiClient>(ConfigureUrl);
        //builder.Services.AddHttpClient();
        builder.Services.AddSingleton<AlertService>();
        //builder.Services.AddSingleton(settings);
        //builder.Services.AddScoped<AuthenticationStateProvider, SmsAuthProvider>();
        //builder.Services.AddScoped<BackOfficeClient.Client>();
        //builder.Services.AddScoped<NotificationService>();

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

    //private static void Main(string[] args)
    //{
    //    var builder = WebApplication.CreateBuilder(args);

    //    // Add services to the container.
    //    builder.Services.AddRazorComponents()
    //        .AddInteractiveServerComponents();

    //    var app = builder.Build();

    //    // Configure the HTTP request pipeline.
    //    if (!app.Environment.IsDevelopment())
    //    {
    //        app.UseExceptionHandler("/Error", createScopeForErrors: true);
    //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //        app.UseHsts();
    //    }
    //    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    //    app.UseHttpsRedirection();

    //    app.UseAntiforgery();

    //    app.MapStaticAssets();
    //    app.MapRazorComponents<App>()
    //        .AddInteractiveServerRenderMode();

    //    app.Run();
    //}
}