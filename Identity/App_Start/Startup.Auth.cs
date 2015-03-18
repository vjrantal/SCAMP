using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;

namespace Identity
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Tenant = "zbrad.com",
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        /*
                        we use the ClientId from the service that VS2015 configures for us
                        to find this:
                        
                        - go to manage.windowsazure.com
                        - select the "Active Directory" service
                        - 

                        */
                        ValidAudience = "82462670-0893-4317-9f72-d8526cf6f662"
                    },
                });


                < add key = "ida:Tenant" value = "zbrad.com" />
       < add key = "ida:Audience" value = "82462670-0893-4317-9f72-d8526cf6f662" />
          < add key = "ida:Password" value = "gtwdpJMghTwWPNzDQQsUk5fo5cyNCXUHg3LK827LcZ4=" />
             < add key = "ida:ClientId" value = "82462670-0893-4317-9f72-d8526cf6f662" />

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "000000000000000",
            //   appSecret: "000000000000000");

            //app.UseGoogleAuthentication(
            //     clientId: "000000000000000.apps.googleusercontent.com",
            //     clientSecret: "000000000000000");
        }
    }
}
