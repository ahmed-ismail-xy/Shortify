using Asp.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shortify.Application.Abstractions.Clock;
using Shortify.Domain.Abstractions;
using Shortify.Domain.Abstractions.Repository;
using Shortify.Infrastructure.Clock;
using Shortify.Infrastructure.Interceptors;

namespace Shortify.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPersistence(services, configuration);

        services.AddDatabaseContext(configuration);

        AddApiVersioning(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddSingleton<ConcurrencyInterceptor>();

        services.AddSingleton<DomainEventsInterceptor>();

        services.AddSingleton<EntityTimestampsInterceptor>();

        services.AddSingleton<SoftDeleteInterceptor>();


        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServerConnection");
        
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString)
                   .AddInterceptors(
                       serviceProvider.GetRequiredService<EntityTimestampsInterceptor>(),
                       serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
                       serviceProvider.GetRequiredService<DomainEventsInterceptor>(),
                       serviceProvider.GetRequiredService<ConcurrencyInterceptor>()
                   );
        });
    }
}
