using Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service
{
    public interface IAuthGoogleService
    {
        string GetSSOLoginUrl(string? state = null);
        Task<TokenResponse?> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default);
        Task<GoogleUserInfo?> GetUserInfoAsync(string accessToken, CancellationToken ct = default);
    }

    public class AuthGoogleService : IAuthGoogleService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public AuthGoogleService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public string GetSSOLoginUrl(string? state = null)
        {
            var q = HttpUtility.ParseQueryString(string.Empty);

            var clientId = _configuration["GoogleOAuth:ClientId"];

            if (string.IsNullOrEmpty(clientId))
                throw new NullReferenceException("clientId is null!");

            var callBackUrl = _configuration["GoogleOAuth:CallBackUrl"];

            if (string.IsNullOrEmpty(callBackUrl))
                throw new NullReferenceException("callBackUrl is null!");

            var endPoint = _configuration["GoogleOAuth:EndPoint"];

            if (string.IsNullOrEmpty(endPoint))
                throw new NullReferenceException("endPoint is null!");

            q["client_id"] = clientId;
            q["redirect_uri"] = callBackUrl;
            q["response_type"] = "code";
            q["scope"] = "openid profile email";
            q["access_type"] = "offline";
            q["prompt"] = "consent";
            if (!string.IsNullOrWhiteSpace(state))
                q["state"] = state;

            return $"{endPoint}?{q}";
        }

        public async Task<TokenResponse?> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default)
        {
            var clientId = _configuration["GoogleOAuth:ClientId"];

            if (string.IsNullOrEmpty(clientId))
                throw new NullReferenceException("clientId is null!");

            var clientSecret = _configuration["GoogleOAuth:Secret"];

            if (string.IsNullOrEmpty(clientSecret))
                throw new NullReferenceException("clientSecret is null!");


            var callBackUrl = _configuration["GoogleOAuth:CallBackUrl"];

            if (string.IsNullOrEmpty(callBackUrl))
                throw new NullReferenceException("callBackUrl is null!");

            var endPoint = _configuration["GoogleOAuth:TokenEndPoint"];

            if (string.IsNullOrEmpty(endPoint))
                throw new NullReferenceException("endPoint is null!");

            var body = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["redirect_uri"] = callBackUrl,
                ["grant_type"] = "authorization_code"
            };

            using var resp = await _http.PostAsync(endPoint, new FormUrlEncodedContent(body), ct);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync(ct);

            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<TokenResponse>(json, opts);
        }

        public async Task<GoogleUserInfo?> GetUserInfoAsync(string accessToken, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, "https://openidconnect.googleapis.com/v1/userinfo");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var resp = await _http.SendAsync(req, ct);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync(ct);

            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<GoogleUserInfo>(json, opts);
        }
    }
}
