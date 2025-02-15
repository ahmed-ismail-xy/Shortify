using Shortify.Domain.Abstractions;
using Shortify.Domain.Entities;

namespace Shortify.Domain.DomainEvents;

public record UrlMappingCreatedEvent : IDomainEvent
{
    public UrlMapping UrlMapping { get; }

    public UrlMappingCreatedEvent(UrlMapping urlMapping)
    {
        UrlMapping = urlMapping;
    }
}
