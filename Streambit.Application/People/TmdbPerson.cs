namespace Streambit.Application.People;

public sealed record TmdbPerson(
    string ExternalId,
    string Name,
    bool Adult,
    int Gender,
    string KnownForDepartment,
    double Popularity,
    string ProfilePath,
    string PhotoUrl,
    IReadOnlyList<TmdbKnownFor> KnownFor);

public sealed record TmdbKnownFor(
    string ExternalId,
    string MediaType,
    bool Adult,
    string BackdropPath,
    string PosterPath,
    IReadOnlyList<int> GenreIds,
    string OriginalLanguage,
    string OriginalTitle,
    string OriginalName,
    string Overview,
    string ReleaseDate,
    string FirstAirDate,
    string Title,
    string Name,
    bool Video,
    double VoteAverage,
    int VoteCount,
    IReadOnlyList<string> OriginCountries);

public sealed record TmdbPopularPeoplePage(int Page, int TotalPages, IReadOnlyList<TmdbPerson> Results);
