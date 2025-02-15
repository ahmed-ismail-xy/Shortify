using Shortify.Domain.Abstractions;
using Shortify.Domain.DomainEvents;
using Shortify.Domain.ValueObjects;

namespace Shortify.Domain.Entities;

public class UrlMapping : Entity
{
    public ShortUrlCode Code { get; private set; }
    public OriginalUrl Destination { get; private set; }
    public Guid OwnerId { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public int Clicks { get; private set; }
    public bool IsActive { get; private set; }

    private UrlMapping() { }

    private UrlMapping(
        ShortUrlCode code,
        OriginalUrl destination,
        Guid ownerId,
        DateTime? expiresAt)
    {
        Code = code;
        Destination = destination;
        OwnerId = ownerId;
        ExpiresAt = expiresAt;
        Clicks = 0;
        IsActive = true;

        RaiseDomainEvent(new UrlMappingCreatedEvent(this));
    }

    public static Result<UrlMapping> Create(
        ShortUrlCode code,
        OriginalUrl destination,
        Guid ownerId,
        DateTime? expiresAt = null)
    {
        var mapping = new UrlMapping(
            code,
            destination,
            ownerId,
            expiresAt);

        return mapping;
    }

    public void IncrementClicks()
    {
        Clicks++;
        RaiseDomainEvent(new UrlClickedEvent(Id, Code.Value));
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        RaiseDomainEvent(new UrlMappingDeactivatedEvent(Id));
    }

    public bool IsExpired() =>
        ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
}
