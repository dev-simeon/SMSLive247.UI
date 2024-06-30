using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLive247.Authentication
{
    public class SmsAuthProvider(ILocalStorageService storage) : AuthenticationStateProvider
    {
        private readonly string storageKey = "UserSession";
        private readonly AuthenticationState anonymousState = new(new(new ClaimsIdentity()));

        public class UserClaims(string email, string apiKey)
        {
            public string Email { get; private set; } = email;
            public string ApiKey { get; private set; } = apiKey;
            //public string? Username { get; private set; }
            //public string? FirstName { get; private set; }
            //public string? LastName { get; private set; }
            //public decimal SmsBalance { get; private set; }
            //public string? FullName { get; private set; }
            //public string? AvatarUrl { get; private set; }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await storage.GetItemAsync<UserClaims>(storageKey);

                if (response == null)
                    return anonymousState;

                return CreateAuthenticationState(response);
            }
            catch
            {
                return anonymousState;
            }
            
        }

        public async Task SaveAuthenticationState(UserClaims member)
        {
            await storage.SetItemAsync(storageKey, member);

            var authenticatedState = CreateAuthenticationState(member);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));
        }

        public ValueTask<UserClaims?> GetMember()
        {
            return storage.GetItemAsync<UserClaims>(storageKey);

        }

        public async Task ClearAuthenticationState()
        {
            await storage.RemoveItemAsync(storageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
        }

        private static AuthenticationState CreateAuthenticationState(UserClaims member)
        {
            var principal = GetClaimsPrincipal(member);
            return new AuthenticationState(principal);
        }

        private static ClaimsPrincipal GetClaimsPrincipal(UserClaims member)
        {
            List<Claim> claims =
                [
                new(ClaimTypes.Email, member.Email),
                new ("SmsBalance", member.ApiKey),
                new ("Key", member.ApiKey)
            ];
            var identity = new ClaimsIdentity(claims, "JwtAuth");
            return new ClaimsPrincipal(identity);
        }
    }
}
