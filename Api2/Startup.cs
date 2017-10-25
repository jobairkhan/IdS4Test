﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               
                })
                    .AddIdentityServerAuthentication(opt =>
                   {
                       opt.RequireHttpsMetadata = false;
                       opt.Authority = "http://localhost:65535"; // IdentityProvider => port running IDP on
                       opt.ApiName = "teachers_test_planner"; // IdentityProvider => api resource name
                      
                   });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(); // Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication(); // The missing line
            
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
