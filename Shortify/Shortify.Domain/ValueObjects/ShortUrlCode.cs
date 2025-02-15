using Shortify.Domain.Abstractions;
using Shortify.Domain.Abstractions.Errors;

namespace Shortify.Domain.ValueObjects;

public record ShortUrlCode
{
    public string Value { get; }

    private ShortUrlCode(string value)
    {
        Value = value;
    }

    public static Result<ShortUrlCode> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation("Short URL code cannot be empty");
        }

        if (value.Length < 4)
        {
            return Error.Validation("Short URL code must be at least 4 characters");
        }

        return new ShortUrlCode(value);
    }
}
