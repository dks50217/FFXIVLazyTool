using Core.Helper;
using Core.Helper.Core.Helper;
using Core.Model;
using Core.Repository;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Service
{
    public interface IAuthService
    {
        string GetSSOLoginUrl();
        Task<TokenResponse?> ExchangeCodeForTokenAsync(string code);
        Task<MicrosoftUserInfo?> GetUserInfoAsync(string accessToken);
    }

    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;

        public AuthService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public string GetSSOLoginUrl()
        {
            var endPoint = _configuration["AzureOAuth:Endpoint"];

            if (string.IsNullOrEmpty(endPoint)) 
                throw new NullReferenceException("endpoint is null!");

            var callBackUrl = _configuration["AzureOAuth:CallBackUrl"];

            if (string.IsNullOrEmpty(callBackUrl))
                throw new NullReferenceException("callBackUrl is null!");

            var clientId = _configuration["AzureOAuth:ClientId"];

            var fullUrl = $"{endPoint}/authorize?scope=openid https://graph.microsoft.com/user.read&redirect_uri={callBackUrl}&response_type=code&client_id={clientId}&state=hos&prompt=consent";

            return fullUrl;
        }

        public async Task<TokenResponse?> ExchangeCodeForTokenAsync(string code)
        {
            var endPoint = _configuration["AzureOAuth:TokenEndPolint"];

            if (string.IsNullOrEmpty(endPoint))
                throw new NullReferenceException("endpoint is null!");

            var redirect_uri = _configuration["AzureOAuth:CallBackUrl"];

            if (string.IsNullOrEmpty(redirect_uri))
                throw new NullReferenceException("redirect_uri is null!");

            var clientId = _configuration["AzureOAuth:ClientId"];

            if (string.IsNullOrEmpty(clientId))
                throw new NullReferenceException("clientId is null!");

            var clientSecret = _configuration["AzureOAuth:Secret"];

            if (string.IsNullOrEmpty(clientSecret))
                throw new NullReferenceException("clientSecret is null!");

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = redirect_uri,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            });

            var response = await _httpClient.PostAsync(endPoint, content);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to get token: {json}");
                return null;
            }

            return JsonSerializer.Deserialize<TokenResponse>(json);
        }

        public async Task<MicrosoftUserInfo?> GetUserInfoAsync(string accessToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to get user info: {json}");
                return null;
            }

            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<MicrosoftUserInfo>(json, opts);
        }
    }
}
