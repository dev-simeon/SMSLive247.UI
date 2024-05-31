using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLive247.Authentication
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddGibsAuthProvider(this IServiceCollection services)
        {
            return services.AddScoped<AuthenticationStateProvider, GibsAuthProvider>();
        }
    }
}
