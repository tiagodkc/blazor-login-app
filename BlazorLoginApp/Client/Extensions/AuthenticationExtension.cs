using Blazored.SessionStorage;
using BlazorLoginApp.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorLoginApp.Client.Extensions
{
    public class AuthenticationExtension : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private ClaimsPrincipal _emptyClaim = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthenticationExtension(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public async Task UpdateAuthenticateState(SessionDTO? sessionDTO)
        {
            ClaimsPrincipal claimsPrincipal;

            if (sessionDTO != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, sessionDTO.Name),
                    new Claim(ClaimTypes.Role, sessionDTO.Role),
                    new Claim("Test", "test example")
                }, authenticationType: "JwtAuth"));

                await _sessionStorageService.SaveStorage("UserSession", sessionDTO);
            }
            else
            {
                claimsPrincipal = _emptyClaim;
                await _sessionStorageService.RemoveItemAsync("UserSession");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userSession = await _sessionStorageService.GetStorage<SessionDTO>("UserSession");

            if (userSession == null)
            {
                return await Task.FromResult(new AuthenticationState(_emptyClaim));
            }

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.Name),
                    new Claim(ClaimTypes.Role, userSession.Role),
                    new Claim("Test", "test example")
                }, authenticationType: "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
    }
}
