namespace Streambit.Api.Tmdb;

public class TmdbOptions
{
    public const string SectionName = "Tmdb";

    public string ApiKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = "https://api.themoviedb.org/3";

    public string Language { get; set; } = "en-US";
}
