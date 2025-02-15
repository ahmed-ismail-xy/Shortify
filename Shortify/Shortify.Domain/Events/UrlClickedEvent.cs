using Shortify.Domain.Abstractions;

namespace Shortify.Domain.DomainEvents;

public record UrlClickedEvent : IDomainEvent
{
    public Guid UrlMappingId { get; }
    public string ShortCode { get; }

    public UrlClickedEvent(Guid urlMappingId, string shortCode)
    {
        UrlMappingId = urlMappingId;
        ShortCode = shortCode;
    }
}
