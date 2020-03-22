using eTutoring.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(eTutoring.Startup))]
namespace eTutoring
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private static void ConfigureOAuth(IAppBuilder app)
        {
            var oauthOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };
            var bearerOptions = new OAuthBearerAuthenticationOptions();

            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(bearerOptions);
        }
    }
}