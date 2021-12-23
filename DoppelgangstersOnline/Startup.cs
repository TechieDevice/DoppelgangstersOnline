using DoppelgangstersOnline.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoppelgangstersOnline.Database;
using DoppelgangstersOnline.Services.Interfaces;
using DoppelgangstersOnline.Services;
using DoppelgangstersOnline.Dtos;
using DoppelgangstersOnline.GameComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DoppelgangstersOnline
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<Settings>();
            var connection = Settings.GetConnectionString();

            services.AddDbContext<ApplicationContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 27))));

            services.AddControllers();
            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,

                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,

                    ValidateLifetime = true,

                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(new[] { "http://localhost:3000", "http://localhost:44334", "http://172.28.112.1:3000", "http://172.28.112.1:44334" })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            services.AddScoped<IUserService, UserAuthService>();

            services.AddSingleton<IDictionary<string, Room>>(opts => new Dictionary<string, Room>());
            services.AddSingleton<IDictionary<string, Player>>(opts => new Dictionary<string, Player>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.Migrate();
            }

            app.UseCors(options => options
                .WithOrigins(new[] { "http://localhost:3000", "http://localhost:44334", "http://172.28.112.1:3000", "http://172.28.112.1:44334" })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapHub<GameHub>("/api/game");
                endpoints.MapDefaultControllerRoute();
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        //spa.UseReactDevelopmentServer(npmScript: "start");
            //        spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            //    }
            //    else
            //    {
            //        spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            //    }
            //});

            
        }
    }
}
