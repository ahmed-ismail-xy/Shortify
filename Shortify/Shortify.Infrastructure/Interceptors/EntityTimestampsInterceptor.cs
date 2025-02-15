using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shortify.Application.Abstractions.Clock;
using Shortify.Domain.Abstractions;

namespace Shortify.Infrastructure.Interceptors;

public sealed class EntityTimestampsInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public EntityTimestampsInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        HandleTimestamps(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleTimestamps(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreationTime(_dateTimeProvider.UtcNow);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetCreationTime(_dateTimeProvider.UtcNow);
                    break;
            }
        }
    }
}
