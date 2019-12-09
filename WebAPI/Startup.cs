using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI.Auths;

namespace WebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["Id4:Authority"];
                    options.RequireHttpsMetadata = false;

                    options.Audience = Configuration["Id4:Audience"];

                   
                    //基于claim的鉴权
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RoleClaimType = "role",
                        NameClaimType= "preferred_username",                        
                    };
                });
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            //services.AddAuthorization(options => { options.AddPolicy("Users", policy => policy.RequireRole("Users")); });

            services.AddAuthorization(option => {
                option.AddPolicy("customs", builder =>
                {
                    //自定义Policy
                    builder.AddRequirements(new CustomsAuthorizationRequirement());
                });
            });
            //注入自定义Policy
            services.AddSingleton<IAuthorizationHandler, CustomsAuthorizationHandler>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //UseAuthentication
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
