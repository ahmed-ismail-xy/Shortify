using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shortify.Application.Abstractions.Clock;
using Shortify.Domain.Abstractions;

namespace Shortify.Infrastructure.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public SoftDeleteInterceptor(IDateTimeProvider dateTimeProvider)
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

        HandleSoftDeletes(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleSoftDeletes(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.Delete(_dateTimeProvider.UtcNow);
                entry.State = EntityState.Modified;
            }
        }
    }
}
