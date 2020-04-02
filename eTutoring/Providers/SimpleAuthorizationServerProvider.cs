using eTutoring.Repositories;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var repo = new AuthRepository())
            {
                var user = await repo.FindUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                var roles = await repo.GetUserRolesById(user.Id);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, context.UserName),
                    new Claim(ClaimTypes.Role, roles.First()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                identity.AddClaims(claims);

                context.Validated(identity);
            }
        }
    }
}