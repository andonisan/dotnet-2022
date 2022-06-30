using System.Data.Common;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TodoApp.App.DependencyInjection;
using TodoApp.App.Infrastructure.Persistence;
using TodoApp.App.IntegrationEventsModule.Persistence;
using TodoApp.Host.DependencyInjection;

namespace TodoApp.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppServices(_configuration);
            
            services.AddScoped<DbConnection>(_ =>
            {
                var connectionString = _configuration.GetConnectionString("SqlServer");
                var connection = new SqlConnection(connectionString);
                return connection;
            });
           
            services.AddDbContext<TodoDbContext>();
            
            services.AddDbContext<IntegrationEventContext>();
            
            services.AddDbContext<ReadOnlyTodoDbContext>();

            services.AddOpenApi();

            services.AddHttpClient();

            services.AddHealthChecks()
                .AddCheck("self", () => new HealthCheckResult(HealthStatus.Healthy));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
            }

            app.UseProblemDetails()
                .UseHttpsRedirection()
                .UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFeatures();
                endpoints.MapHealthChecks();
                endpoints.MapControllers();
            });
        }
    }
}