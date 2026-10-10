namespace Streambit.Api.Tmdb;

public class GenreSyncOptions
{
    public const string SectionName = "GenreSync";

    public bool Enabled { get; set; } = true;

    public TimeSpan Interval { get; set; } = TimeSpan.FromHours(12);
}
