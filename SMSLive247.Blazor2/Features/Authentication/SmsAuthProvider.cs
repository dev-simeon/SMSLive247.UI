using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace SMSLive247.Authentication
{
    public class SmsAuthProvider(ProtectedLocalStorage storage) : AuthenticationStateProvider
    {
        private Guid? _cachedApiKey;
        private readonly string storageKey = "UserSession";
        private readonly AuthenticationState anonymousState = new(new(new ClaimsIdentity()));

        public Guid? GetApiKey() => _cachedApiKey;

        public async Task SaveAuthenticationState(Guid apiKey)
        {
            _cachedApiKey = apiKey;
            await storage.SetAsync(storageKey, apiKey);

            var authenticatedState = CreateAuthenticationState(apiKey);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));
        }

        public async Task ClearAuthenticationState()
        {
            _cachedApiKey = null;
            await storage.DeleteAsync(storageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await storage.GetAsync<Guid>(storageKey);

                if (!result.Success || result.Value == default)
                    return anonymousState;

                _cachedApiKey = result.Value;
                return CreateAuthenticationState(result.Value);
            }
            catch
            {
                return anonymousState;
            }
        }

        private static AuthenticationState CreateAuthenticationState(Guid apiKey)
        {
            var principal = GetClaimsPrincipal(apiKey);
            return new AuthenticationState(principal);
        }

        private static ClaimsPrincipal GetClaimsPrincipal(Guid apiKey)
        {
            List<Claim> claims = [
                new ("Key", apiKey.ToString())
            ];
            var identity = new ClaimsIdentity(claims, "JwtAuth");
            return new ClaimsPrincipal(identity);
        }
    }
}
