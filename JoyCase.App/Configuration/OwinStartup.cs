using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(JoyCase.App.OwinStartup))]

namespace JoyCase.App
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new Microsoft.Owin.PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });
        }
    }
}
