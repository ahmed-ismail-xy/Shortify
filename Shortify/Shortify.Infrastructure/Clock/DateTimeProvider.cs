using Shortify.Application.Abstractions.Clock;

namespace Shortify.Infrastructure.Clock;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}