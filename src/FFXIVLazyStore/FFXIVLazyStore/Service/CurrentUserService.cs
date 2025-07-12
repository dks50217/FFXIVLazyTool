using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FFXIVLazyStore.Service
{
    public interface ICurrentUserService
    {
        Task<ClaimsPrincipal> GetUserAsync();
    }


    public class CurrentUserService : ICurrentUserService
    {
        private readonly AuthenticationStateProvider _provider;

        public CurrentUserService(AuthenticationStateProvider provider)
        {
            _provider = provider;
        }

        public async Task<ClaimsPrincipal> GetUserAsync()
        {
            var state = await _provider.GetAuthenticationStateAsync();
            return state.User;
        }
    }
}
