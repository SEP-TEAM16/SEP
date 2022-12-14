using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SEP.Gateway.Controllers;
using System.Text;

namespace SEP.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IConfiguration OcelotConfiguration { get; }

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
                   .AddJsonFile("microservices.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();

            OcelotConfiguration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication().AddJwtBearer("auth_scheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("some_big_key_value_here_secret")),
                    ValidAudience = "audience",
                    ValidIssuer = "issuer",
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            });

            services.AddOcelot(OcelotConfiguration);

            services.AddSingleton<AuthController, AuthController>();
            services.AddMvc().AddControllersAsServices();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot().Wait();
        }
    }
}