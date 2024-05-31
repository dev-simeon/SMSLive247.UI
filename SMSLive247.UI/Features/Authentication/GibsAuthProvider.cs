using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
//using SMSLive247.OpenApi;
using SMSLive247.ApiClient.OpenAPIService;

namespace SMSLive247.Authentication
{
    public class GibsAuthProvider(ILocalStorageService storage) : AuthenticationStateProvider
    {
        private readonly string storageKey = "UserSession";
        private readonly AuthenticationState anonymousState = new(new(new ClaimsIdentity()));
        public class UserClaims
        {
            public string? Username { get; init; }  
            public string? Email { get; init; }
            public string? FirstName { get; init; }
            public string? LastName { get; init; }
            public string? FullName { get; init; }  
            public string? AvatarUrl { get; init; }  
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await storage.GetItemAsync<LoginResponse>(storageKey);

                if (response == null)
                    return anonymousState;

                return CreateAuthenticationState(response.AccessToken);
            }
            catch 
            {
                return anonymousState;
            }
        }

        public async Task<UserClaims> GetUserClaims()
        {
            var response = await storage.GetItemAsync<LoginResponse>(storageKey) 
                ?? throw new InvalidOperationException("Please login");

            var principal = GetClaimsPrincipal(response.AccessToken);




            //var key = Encoding.ASCII.GetBytes("510B3486-3BE4-4D38-9F48-4T2B-0000-42A5-B2F9");
            //var handler = new JwtSecurityTokenHandler();

            //handler.ValidateToken(response.AccessToken, new TokenValidationParameters
            //{
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(key),
            //    ValidateIssuer = false,
            //    ValidateAudience = false,
            //    ClockSkew = TimeSpan.Zero
            //}, out SecurityToken validatedToken);

            //var jwtToken = (JwtSecurityToken)validatedToken;
            //var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);





            return new UserClaims
            {
                Email = principal.Claims.FirstOrDefault(x => x.Type == "email")?.Value,
                Username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value,
                FullName = principal.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value,
                AvatarUrl = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Uri)?.Value,
                LastName = principal.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value,
                FirstName = principal.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value,
            };

            //return new UserClaims
            //{
            //    Email     = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
            //    Username  = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value,
            //    FullName  = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
            //    AvatarUrl = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Uri)?.Value,
            //    LastName  = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
            //    FirstName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
            //};
        }

        public async Task<string?> GetAccessToken()
        {
            var response = await storage.GetItemAsync<LoginResponse>(storageKey);

            if (response != null)
                return response.AccessToken;

            return null;
        }

        public async Task SaveAuthenticationState(LoginResponse response)
        {
            await storage.SetItemAsync(storageKey, response); 

            var authenticatedState = CreateAuthenticationState(response.AccessToken);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));
        }

        public async Task ClearAuthenticationState()
        {
            await storage.RemoveItemAsync(storageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
        }

        private static AuthenticationState CreateAuthenticationState(string accessToken)
        {
            var principal = GetClaimsPrincipal(accessToken);
            return new AuthenticationState(principal);
        }

        private static ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var identity = new ClaimsIdentity(token.Claims, "JwtAuth");

            return new ClaimsPrincipal(identity);
        }

    }
}