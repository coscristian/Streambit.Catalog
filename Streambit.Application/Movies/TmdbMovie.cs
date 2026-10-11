namespace Streambit.Application.Movies;

public sealed record TmdbMovie(
    string ExternalId,
    string Title,
    string OriginalTitle,
    string OriginalLanguage,
    string Overview,
    string ReleaseDate,
    double Popularity,
    double VoteAverage,
    int VoteCount,
    bool Adult,
    bool Video,
    string PosterPath,
    string PosterUrl,
    string BackdropPath,
    string BackdropUrl,
    IReadOnlyList<int> GenreIds);

public sealed record TmdbDiscoverMoviesPage(int Page, int TotalPages, IReadOnlyList<TmdbMovie> Results);
