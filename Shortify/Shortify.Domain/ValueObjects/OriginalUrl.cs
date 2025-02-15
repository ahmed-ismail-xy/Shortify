using Shortify.Domain.Abstractions;
using Shortify.Domain.Abstractions.Errors;

namespace Shortify.Domain.ValueObjects;

public record OriginalUrl
{
    public string Value { get; }

    private OriginalUrl(string value)
    {
        Value = value;
    }

    public static Result<OriginalUrl> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation("URL cannot be empty.");
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
        {
            return Error.Validation("Invalid URL format.");
        }

        return new OriginalUrl(value);
    }
}
