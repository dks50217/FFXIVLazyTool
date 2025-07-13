using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Core.Model;
using Core.Service;
using Service.Service;
using Core.Helper;
using Core.Helper.Core.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FFXIVLazyStore.Controllers
{
    [Route("[controller]")]
    public class SSOController : Controller
    {
        private readonly IAuthAzureService _authAzureService;
        private readonly IAuthGoogleService _authGoogleService;

        public SSOController(IAuthAzureService authService, IAuthGoogleService authGoogleService)
        {
            _authAzureService = authService;
            _authGoogleService = authGoogleService;
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallBack(string? code, string? error)
        {
            if (string.IsNullOrEmpty(code))
                return Redirect("/");

            var token = await _authAzureService.ExchangeCodeForTokenAsync(code: code);

            if (token is null)
                throw new NullReferenceException("token is null!");

            if (string.IsNullOrEmpty(token.access_token))
                throw new NullReferenceException("access_token is null!");

            var userinfo = await _authAzureService.GetUserInfoAsync(accessToken: token.access_token);

            if (userinfo == null)
                return Unauthorized("User info retrieval failed");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userinfo.Id ?? ""),
                new Claim(ClaimTypes.Name, userinfo.DisplayName ?? ""),
                new Claim(ClaimTypes.Email, userinfo.Mail ?? userinfo.UserPrincipalName ?? ""),
                new Claim("access_token", token.access_token)
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 principal,
                 new AuthenticationProperties
                 {
                     IsPersistent = true,
                     ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                 });

            return Redirect("/");
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback(
            string code,
            string? state,
            CancellationToken ct)
        {
            var token = await _authGoogleService.ExchangeCodeForTokenAsync(code, ct);

            if (token is null) 
                return Unauthorized("Token error");

            if (string.IsNullOrEmpty(token.access_token))
                return Unauthorized("Token is empty");

            var userinfo = await _authGoogleService.GetUserInfoAsync(token.access_token, ct);

            if (userinfo is null) 
                return Unauthorized("User info error");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userinfo.Id ?? ""),
                new Claim(ClaimTypes.Name, userinfo.Name ?? ""),
                new Claim(ClaimTypes.Email, userinfo.Email ?? userinfo.Name ?? ""),
                new Claim("access_token", token.access_token)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            return Redirect("/");
        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
