using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shortify.Api.Middleware;
using Shortify.Infrastructure;

namespace Shortify.Api.Extensions;

internal static class WebApplicationExtensions
{
    public static void UseAppDefaults(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        app.UseSwaggerUIForVersions();

        app.UseCors("AllowAll");

        app.UseHttpsRedirection();

        app.UseMiddleware<RequestContextLoggingMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();
    }

    public static void UseSwaggerUIForVersions(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (ApiVersionDescription description in app.DescribeApiVersions())
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.EnsureCreated();

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
