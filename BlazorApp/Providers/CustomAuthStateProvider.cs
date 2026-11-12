using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace SampleApp.Client.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Token ရှိလျှင် Claims များကို ဖြည်ထုတ်ပြီး Login ဝင်ထားသူအဖြစ် သတ်မှတ်ပါ
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        // Login အောင်မြင်ချိန်တွင် ခေါ်ရန်
        public void NotifyUserLoggedIn(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            NotifyAuthenticationStateChanged(authState);
        }

        // Logout လုပ်ချိန်တွင် ခေါ်ရန်
        public void NotifyUserLoggedOut()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }

        // JWT Token (Base64) ကို C# က နားလည်သော Claims များအဖြစ် ဖြည်ထုတ်ပေးသည့် Helper Method
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)
                ?? new Dictionary<string, object>();

            // JWT ထဲက short claim names ("role", "email", "sub") ကို .NET ရဲ့
            // standard ClaimTypes များအဖြစ် ပြောင်းပေးခြင်း။
            // ဒါမှ [Authorize(Roles = "...")] က Role ကို မှန်ကန်စွာ စစ်နိုင်မည်။
            return keyValuePairs.Select(kvp =>
            {
                var type = kvp.Key switch
                {
                    "role" => ClaimTypes.Role,
                    "email" => ClaimTypes.Email,
                    "sub" => ClaimTypes.NameIdentifier,
                    "name" => ClaimTypes.Name,
                    _ => kvp.Key
                };

                return new Claim(type, kvp.Value?.ToString() ?? string.Empty);
            });
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
}
