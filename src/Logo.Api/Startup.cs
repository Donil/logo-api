using Logo.Api.Configuration;
using Logo.Api.Services;
using Logo.DataAccess;
using Logo.DataAccess.Managers;
using Logo.DataAccess.Models;
using Logo.DataAccess.Repositories;
using Logo.Domain.BusinessCards;
using Logo.Domain.Shape;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logo.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration["NpgsqlServer:ConnectionString"];

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddUserStore<UserStore>()
                .AddUserManager<UserManager>()
                .AddRoleManager<RoleManager>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Enable CORS
            services.AddCors();

            //string url = Configuration["IdentityServerAddress"];
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
            });

            // Add framework services.
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IBusinessCardsRepository, BusinessCardsRepository>();
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<IShapesRepository, ShapesRepositoy>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            InitializeDatabase(app).Wait();

            app.UseStaticFiles();

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }

        private async Task InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                appDbContext.Database.Migrate();

                // Create roles if not exist
                var rolesManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager>();

                foreach (string role in UsersConfiguration.GetRoles())
                {
                    bool roleExist = await rolesManager.RoleExistsAsync(role);
                    if (roleExist) 
                    {
                        continue;
                    }
                    IdentityResult result = await rolesManager.CreateAsync(new ApplicationRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
                    }
                }
                // Create users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager>();
                foreach (CreationUser creationUser in UsersConfiguration.GetUsers())
                {
                    ApplicationUser appUser = await userManager.FindByEmailAsync(creationUser.Email);
                    if (appUser != null)
                    {
                        continue;
                    }
                    await userManager.CreateAsync(creationUser.Email, creationUser.FirstName, 
                                                    creationUser.LastName, creationUser.Password, creationUser.Role);
                }
            }
        }
    }
}
