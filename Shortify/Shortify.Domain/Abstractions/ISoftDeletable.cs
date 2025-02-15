namespace Shortify.Domain.Abstractions;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    void Delete(DateTime deletedAt);
}
