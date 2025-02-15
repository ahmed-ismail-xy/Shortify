using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shortify.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Shortify.Infrastructure.Interceptors;

public sealed class ConcurrencyInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<ConcurrencyInterceptor> _logger;

    public ConcurrencyInterceptor(ILogger<ConcurrencyInterceptor> logger)
    {
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult> ThrowingConcurrencyExceptionAsync(
        ConcurrencyExceptionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(eventData.Exception, "A concurrency exception occurred and a retry will be attempted.");

        foreach (var entry in eventData.Entries)
        {
            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken) 
                ?? throw new ConcurrencyException("A concurrency conflict occurred after saving changes.");

            entry.OriginalValues.SetValues(databaseValues);
            entry.CurrentValues.SetValues(entry.Entity);
        }

        return InterceptionResult.Suppress();
    }
}