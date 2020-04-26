using eTutoring.Providers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;

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
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            // Enable SignalR
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll).RunSignalR();
            });
        }

        protected void ConfigureOAuth(IAppBuilder app)
        {
            var oauthOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/auth/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                Provider = new SimpleAuthorizationServerProvider()
            };
            var bearerOptions = new OAuthBearerAuthenticationOptions();

            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(bearerOptions);
        }
    }
}