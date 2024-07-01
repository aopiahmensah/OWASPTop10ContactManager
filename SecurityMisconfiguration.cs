using Microsoft.VisualBasic;
using System.Configuration;
using System;
using Microsoft.AspNetCore.DataProtection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Identity;
using Humanizer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace OWASPContactManager
{
    //Security misconfiguration is a common vulnerability that arises when security settings are not defined, implemented, or maintained properly.
    //This can include issues like missing security headers, unnecessary services enabled, default accounts, and more.

    //Vulnerable Configuration

    //The following code block shows an example of a vulnerable configuration where security headers are missing, and debug information is
    //exposed in a production environment.

    //Explanation
    //Missing HTTPS Redirection: HTTPS redirection is commented out, making the application susceptible to man-in-the-middle attacks.
    //Missing Security Headers: Important security headers like X-Content-Type-Options, X-Frame-Options, and X-XSS-Protection are missing.
    public class StartupVulneralble
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Insecure: Detailed error messages are shown in development mode
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts(); // Secure: HSTS header is added


            }

            app.UseStaticFiles();

            app.UseRouting();

            // Insecure: Missing security headers
            // Secure: Add security headers middleware

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }

    //Secure Configuration

    //The following code block shows a secure configuration where security headers are properly added, HTTPS redirection is enforced, 
    //and detailed error messages are not exposed in a production environment.

    //Development Mode: Detailed error messages are only shown in development mode using the UseDeveloperExceptionPage middleware.
    //Production Mode: Proper error handling is configured using UseExceptionHandler, and HSTS is enforced using UseHsts.
    //HTTPS Redirection: HTTPS redirection is enforced using UseHttpsRedirection.
    //Security Headers: Important security headers are added using middleware to protect against common attacks like MIME sniffing, clickjacking, and XSS.
    //Strong Password Policies: Configuring strong password policies to enforce security at the authentication level.
    public class StartupSecure
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Secure: Configure strong password policies
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Secure: Detailed error messages are only shown in development mode
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Secure: Proper error handling in production
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // Secure: HSTS header is added

                // Secure: HTTPS redirection
                app.UseHttpsRedirection();
            }

            // Secure: Use security headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                await next();
            });

            app.UseStaticFiles();

            app.UseRouting();

            // Secure: Use authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }

}
