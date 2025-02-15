namespace Shortify.Core.Entities;

public sealed class ShortUrl
{
    public Guid Id { get; set; }
    public string BlockedUrl { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
