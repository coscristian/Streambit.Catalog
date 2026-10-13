using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application;

internal static class CatalogValues
{
    public static ContentFormat? ParseListFormat(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return ContentFormat.All;

        var normalized = NormalizeFormat(value);
        if (normalized.Equals(nameof(ContentFormat.All), StringComparison.OrdinalIgnoreCase))
            return ContentFormat.All;

        if (Enum.TryParse<ContentFormat>(normalized, true, out var format)
            && Enum.IsDefined(format)
            && format != ContentFormat.All)
            return format;

        throw new ArgumentException("Format must be All, Movie, or Series.");
    }

    public static ContentFormat ParseFormat(string? value)
    {
        var normalized = NormalizeFormat(value);
        if (Enum.TryParse<ContentFormat>(normalized, true, out var format)
            && Enum.IsDefined(format)
            && format != ContentFormat.All)
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

    private static string NormalizeFormat(string? value)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (normalized.Equals("Movies", StringComparison.OrdinalIgnoreCase))
            return nameof(ContentFormat.Movie);

        if (normalized.Equals("Serie", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("Series", StringComparison.OrdinalIgnoreCase))
            return nameof(ContentFormat.Series);

        return normalized;
    }
}
