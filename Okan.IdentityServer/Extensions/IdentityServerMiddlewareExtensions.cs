using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Okan.IdentityServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Okan.IdentityServer.Extensions
{
    public static class IdentityServerMiddlewareExtensions
    {
        public static void AddMvcAuthentication(this AuthenticationBuilder appBuilder, IClientService authSettings)
        {
            var clientDetail = authSettings.GetClientDetail();
            var customClaims = authSettings.GetCustomClaims();
            appBuilder.AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = clientDetail.ClientId;
                options.ClientSecret = clientDetail.ClientSecret;
                options.Authority = clientDetail.Authority; //"https://localhost:7267/";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.SignedOutCallbackPath = clientDetail.LogOutAddress;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("roles");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.ClaimActions.MapUniqueJsonKey("gender", "gender", "gender");
                options.TokenValidationParameters.RoleClaimType = "role";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    AuthenticationType = CookieAuthenticationDefaults.AuthenticationScheme
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var identity = context.Principal?.Identity as ClaimsIdentity;
                        if (identity == null)
                        {
                            return;
                        }
                        identity.AddClaims(customClaims);
                    }
                };

            });
        }
    }
}
