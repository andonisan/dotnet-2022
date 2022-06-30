using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TodoApp.App.Persistence;
using TodoApp.Host.DependencyInjection;

namespace TodoApp.Host;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpApiServices(Configuration);

        services.AddOpenApi();

        services.AddDbContext<TodoDbContext>(options =>
        {
            options.UseSqlServer(Configuration["ConnectionStrings:SqlServer"], builder =>
            {
                builder.MigrationsAssembly(typeof(TodoDbContext).Assembly.FullName);
                builder.EnableRetryOnFailure();
            });
        });


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

        app.UseProblemDetails();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks();
        });
    }
}