using Shortify.Domain.Abstractions;

namespace Shortify.Domain.DomainEvents;

public record UrlMappingDeactivatedEvent : IDomainEvent
{
    public Guid UrlMappingId { get; }

    public UrlMappingDeactivatedEvent(Guid urlMappingId)
    {
        UrlMappingId = urlMappingId;
    }
}
