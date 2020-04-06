using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ComplaintApp.Application.Authentication;
using ComplaintApp.Application.Complaint;
using ComplaintApp.Application.Shared;
using ComplaintApp.Core.Users;
using ComplaintApp.EntityFrameworkCore.Migrations;
using ComplaintApp.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ComplaintApp.API
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
            services.AddControllers();
            services.AddDbContext<ComplaintDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    // Added a configuration to ignore warnings in the console
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning)));
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddAutoMapper(typeof(Startup));

            var builder = services.AddIdentityCore<User>(options =>
            {
                // We are configuring this just for development to allow
                // weak passwords we are currently using.  In production,
                // we need to make this strict
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.TryAddSingleton<ISystemClock, SystemClock>();
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            // We are telling Identity that we want to use entity framework as our store
            // And, if we add a new migration, you will see tables created for identity
            builder.AddEntityFrameworkStores<ComplaintDbContext>();
            // A service that checks the roles
            builder.AddRoleValidator<RoleValidator<Role>>();
            // A service that creates and remove roles
            builder.AddRoleManager<RoleManager<Role>>();
            // A service that allows the user to log-in
            builder.AddSignInManager<SignInManager<User>>();

            // Add the Seed class
            services.AddTransient<Seed>();

            // We want to create LogUserActivity per request so we will
            // be using AddScoped
            services.AddScoped<LogUserActivity>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("SecuritySettings:Token").Value)),
                        // Our issuer and audience is localhost so we don't need to validate
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                // These are the policies we have and we are going to keep it simple
                // These policies can be quite flexible, we are just going to use it to 
                // define roles for a specific action in the controller
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole("Admin"));
                options.AddPolicy("ModeratePhotoRole", policy =>
                    policy.RequireRole("Admin", "Moderator"));
                options.AddPolicy("Staff", policy =>
                    policy.RequireRole("Staff"));
                options.AddPolicy("Customer", policy =>
                    policy.RequireRole("Customer"));
            });

            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        // This is going to require authentication globally
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));

                }).SetCompatibilityVersion(CompatibilityVersion.Latest)
                // The below option fixes issues like the below
                // fail: Microsoft.AspNetCore.Server.Kestrel[13]
                // Connection id "0HLH7GM9OPUNC", Request id "0HLH7GM9OPUNC:00000001": An unhandled exception was thrown by the application.
                // Newtonsoft.Json.JsonSerializationException: Self referencing loop detected for property 'user' with type 'DatingApp.API.Models.User'.Path '[0].photos[0]'.
                // This error in Postman is only shown as ==> Expected ',' instead of ''
                .AddNewtonsoftJson();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddCors();
        }

        
        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    // Add the Seed class
        //    services.AddTransient<Seed>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Seed the users
            // Just uncomment this if we ever we need to seed our database
            seeder.SeedUsers();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
