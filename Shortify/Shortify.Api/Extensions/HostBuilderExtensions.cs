using Serilog;
using Serilog.Events;

namespace Shortify.Api.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .WriteTo.Console();

            if (context.HostingEnvironment.IsProduction())
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            }
        });
    }
}
