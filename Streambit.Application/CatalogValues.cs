using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application;

internal static class CatalogValues
{
    public static ContentFormat ParseFormat(string? value)
    {
        if (Enum.TryParse<ContentFormat>(value, true, out var format) && Enum.IsDefined(format))
            return format;

        throw new ArgumentException("Format must be Movie or Series.");
    }

    public static Availability ParseAvailability(string? value)
    {
        var normalized = value?.Replace(" ", "", StringComparison.Ordinal) ?? string.Empty;
        if (normalized.Equals("Nowshowing", StringComparison.OrdinalIgnoreCase))
            return Availability.NowShowing;

        if (normalized.Equals("Comingsoon", StringComparison.OrdinalIgnoreCase))
            return Availability.ComingSoon;

        if (Enum.TryParse<Availability>(normalized, true, out var availability) && Enum.IsDefined(availability))
            return availability;

        throw new ArgumentException("Availability must be NowShowing or ComingSoon.");
    }

    public static CreditRole ParseRole(string? value)
    {
        if (Enum.TryParse<CreditRole>(value, true, out var role) && Enum.IsDefined(role))
            return role;

        throw new ArgumentException("Role must be Lead, Supporting, or Director.");
    }
}
