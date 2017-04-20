namespace Allors.Server
{
    using System;

    using Microsoft.AspNetCore.Builder;

    public static class DefaultAuthenticationAppBuilderExtensions
    {

        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new CookieAuthenticationOptions
                                              {
                                                  AuthenticationScheme = AuthenticationController.Scheme,
                                                  AutomaticAuthenticate = true,
                                                  AutomaticChallenge = false
                                              };

            return app.UseCookieAuthentication(options);
        }
    }
}