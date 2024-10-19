using FFXIVLazyStore.Model;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace FFXIVLazyStore.Components.Shared
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private string Key { get; set; } = "LoginUserInfo";
        private readonly ProtectedLocalStorage protectedLocalStorage;
        private ClaimsPrincipal anonymousPrincipal = new(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage)
        {
            this.protectedLocalStorage = protectedLocalStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await protectedLocalStorage.GetAsync<LoginUserInfo>(Key);

                LoginUserInfo? loginUserInfo = null;

                if (result.Success)
                {
                    loginUserInfo = result.Value;
                }

                if (loginUserInfo is null)
                {
                    return await Task.FromResult(new AuthenticationState(anonymousPrincipal));
                }

                if (LoginExpires(loginUserInfo))
                {
                    return await Task.FromResult(new AuthenticationState(anonymousPrincipal));
                }

                ClaimsIdentity identity = new(new List<Claim> {
                    new Claim(ClaimTypes.Name, loginUserInfo.UserName)
                 }, "CustomAuthentication");

                ClaimsPrincipal principal = new(identity);

                return await Task.FromResult(new AuthenticationState(principal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymousPrincipal));
            }
        }

        public async Task UpdateState(LoginUserInfo loginUserInfo)
        {
            ClaimsPrincipal principal;

            if (loginUserInfo is null)
            {
                await protectedLocalStorage.DeleteAsync(Key);
                principal = anonymousPrincipal;
            }
            else
            {
                await protectedLocalStorage.SetAsync(Key, loginUserInfo);
                ClaimsIdentity identity = new(new List<Claim>
                    {
                      new Claim(ClaimTypes.Name, loginUserInfo.UserName)
                    }, "CustomAuthentication");

                principal = new ClaimsPrincipal(identity);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal!)));
        }

        private bool LoginExpires(LoginUserInfo data) => data.ExpiresAt < DateTimeOffset.Now;
    }
}
