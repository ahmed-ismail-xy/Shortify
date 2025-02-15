using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Shortify.Api.Configurations;
using Shortify.Api.OpenApi;
using Shortify.Application;
using Shortify.Infrastructure;
using System.Text.Json.Serialization;

namespace Shortify.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddApplication(configuration);

        services.AddControllersWithJsonOptions();

        services.AddSwaggerDocumentation();

        services.AddProblemDetails();

        services.AddHealthChecks();

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        services.AddHttpContextAccessor();

        return services;
    }

    private static IServiceCollection AddProblemDetails(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, ShortifyProblemDetailsFactory>();

        return services;
    }

    private static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Shortify API", Version = "v1" });
        });

        return services;
    }

    private static IServiceCollection AddControllersWithJsonOptions(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }
}