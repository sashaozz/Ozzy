using Microsoft.AspNetCore.Builder;

namespace Ozzy.Server.Api.Configuration
{
    public static class OzzyServiceCollectionExtensions
    {
        public static void UseOzzyApi(this IApplicationBuilder app)
        {
            app.UseMvc();            
        }       
    }
}
