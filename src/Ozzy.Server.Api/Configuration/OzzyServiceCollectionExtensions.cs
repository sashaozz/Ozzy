using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Api.Configuration
{
    public static class OzzyServiceCollectionExtensions
    {
        public static void UseOzzyApi(this IApplicationBuilder app)
        {
            app.UseMvc();            
        }

        public static void UseSimpleOzzyAuth(this IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
        }
    }
}
