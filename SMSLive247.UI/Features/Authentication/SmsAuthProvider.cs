using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using SMSLive247.OpenApi;
using System.IdentityModel.Tokens.Jwt;

namespace SMSLive247.Authentication
{
    public class SmsAuthProvider(ILocalStorageService storage) : AuthenticationStateProvider
    {
        private readonly string storageKey = "UserSession";
        private readonly AuthenticationState anonymousState = new(new(new ClaimsIdentity()));

        public class UserClaims
        {
            public string? Username { get; init; }
            public string? Email { get; init; }
            public string? FirstName { get; init; }
            public string? LastName { get; init; }
            public decimal SmsBalance { get; init; }
            public string? FullName { get; init; }
            public string? AvatarUrl { get; init; }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await storage.GetItemAsync<UI.Member>(storageKey);

                if (response == null)
                    return anonymousState;

                return CreateAuthenticationState(response);
            }
            catch
            {
                return anonymousState;
            }
            
        }

        public async Task SaveAuthenticationState(UI.Member member)
        {
            await storage.SetItemAsync(storageKey, member);

            var authenticatedState = CreateAuthenticationState(member);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));
        }

        public ValueTask<UI.Member?> GetMember()
        {
            return storage.GetItemAsync<UI.Member>(storageKey);

        }

        public async Task ClearAuthenticationState()
        {
            await storage.RemoveItemAsync(storageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
        }

        private static AuthenticationState CreateAuthenticationState(UI.Member member)
        {
            var principal = GetClaimsPrincipal(member);
            return new AuthenticationState(principal);
        }

        private static ClaimsPrincipal GetClaimsPrincipal(UI.Member member)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, member.Email),
                new Claim("SmsBalance", member.ApiKey),
                new Claim("Key", member.ApiKey)
            };
            var identity = new ClaimsIdentity(claims, "JwtAuth");

            return new ClaimsPrincipal(identity);
        }
    }
}
