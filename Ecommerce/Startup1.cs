using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(Ecommerce.Startup1))]

namespace Ecommerce
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                //URl http how expirestion
                TokenEndpointPath = new PathString("/login"),//http-https
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(600),
                AllowInsecureHttp = true,
                //how to create token (fields)==>
                Provider = new TokenCreate()

            }); ;
            //Check token valid 
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            app.UseWebApi(config);

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
        internal class TokenCreate : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                //
                context.Validated();//any clientid Valid
            }
            //Check User Password ==>login {"":,""} ==>create Token
            public override async Task GrantResourceOwnerCredentials
                (OAuthGrantResourceOwnerCredentialsContext context)//Method//Request Body /login
            {
                //OWin Cors
                context.OwinContext.Response.Headers.Add(" Access - Control - Allow - Origin ", new[] { "*" });
                //Check

                ApplicationUserManager manager =
                    new ApplicationUserManager(new ApplicationDBContext());
                ApplicationIdentityUser user = await manager.FindAsync(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("grant_error", "username & password Not Valid");//invalid
                }
                else
                {
                    //create token
                    ClaimsIdentity claims = new ClaimsIdentity(context.Options.AuthenticationType);//token beare -jwt cookie
                                                                                                   //fields 
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    if (manager.IsInRole(user.Id, "Admin"))
                        claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

                    context.Validated(claims);//card field NAme,image,...role
                }
                //fields Token
            }
        }
    }
}
