namespace Shortify.Domain.Abstractions;

public abstract class Entity
{
    protected Entity() { }

    protected Entity(Guid id)
    {
        Id = id;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    internal void SetCreationTime(DateTime createdAt)
    {
        if (CreatedAtUtc != default) return;
        CreatedAtUtc = createdAt;
    }

    internal void SetUpdateTime(DateTime updatedAt)
    {
        UpdatedAtUtc = updatedAt;
    }
}
