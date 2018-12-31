using System;
using System.Text;
using BLL.Models;
using DAL;
using DAL.Entities;
using InternetHospital.WebApi.CustomMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //allow to get user Id in BLL
            services.AddHttpContextAccessor();

            // enable CORS
            services.AddCors();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // configure entity framework
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<ApplicationContext>(opt =>
                    {
                        opt.UseSqlServer(Configuration.GetConnectionString("LocaleMsSql"),
                                      m => m.MigrationsAssembly("WebAPI"));
                    });
            
            // configure identity
            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.SignIn.RequireConfirmedEmail = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = false;
            })
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            // configure jwt authentication         

            // for futher usage

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuer = true,
            //                ValidateAudience = false,
            //                ValidateLifetime = true,
            //                ValidateIssuerSigningKey = true,
            //                ValidIssuer = appSettings.JwtIssuer,
            //                ClockSkew = TimeSpan.Zero,
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtKey))
            //            };
            //        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseCors(options => options.AllowAnyOrigin()
                                          .AllowAnyMethod()
                                          .AllowAnyHeader()
                                          .AllowCredentials());
            // app.UseAuthentication();
            app.UseMvc();
        }
    }
}
