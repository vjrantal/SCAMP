﻿using System;
using System.Linq;
using AutoMapper;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using KeyVaultRepositories.Implementation;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ScampApi.Infrastructure;
using Microsoft.AspNet.StaticFiles;
using ProvisioningLibrary;
using ScampTypes.ViewModels;
using IConfiguration = Microsoft.Framework.ConfigurationModel.IConfiguration;
using System.IdentityModel.Tokens;

namespace ScampApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables("APPSETTING_");

        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                var jsonFormatter = (JsonOutputFormatter)(options.OutputFormatters
                    .First(formatter => formatter.Instance is JsonOutputFormatter)
                    .Instance);
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //options.Filters.Add(new RequireHttpsAttribute());

            });
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILinkHelper, LinkHelper>();
            
            services.AddScoped<ISecurityHelper, SecurityHelper>();

            services.AddSingleton <IWebJobController, WebJobController>();
            services.AddSingleton<IVolatileStorageController, VolatileStorageController>();
            services.AddSingleton<IGraphAPIProvider, GraphAPIProvider>();

            services.AddInstance(Configuration);

            services.AddDocumentDbRepositories(Configuration);
            services.AddKeyVaultRepositories(Configuration);
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOAuthBearerAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = false
                };

                options.Audience = Configuration.Get("ClientId");
                options.Authority = String.Format(Configuration.Get("AadInstance"), Configuration.Get("TenantId"));
            });

            // Required by kestrel and web commands.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
                
            });
            //Configure AutoMapper
            Mapper.CreateMap<ScampResource, ScampResourceSummary>();
            Mapper.CreateMap<User, UserSummary>();
            Mapper.CreateMap<User, ScampUserReference>();
        }
    }
}
