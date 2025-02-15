using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shortify.Domain.Abstractions;

namespace Shortify.Infrastructure.Interceptors;

public sealed class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _domainEventPublisher;

    public DomainEventsInterceptor(IPublisher domainEventPublisher)
    {
        _domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .ToList();

        foreach (var entity in domainEntities)
        {
            var domainEvents = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _domainEventPublisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
