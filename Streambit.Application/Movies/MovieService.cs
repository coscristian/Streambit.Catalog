using Streambit.Application.Abstractions;
using Streambit.Contracts.Genres;
using Streambit.Contracts.Movies;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application.Movies;

public class MovieService : IMovieService
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    private readonly IMovieRepository _movies;
    private readonly IGenreRepository _genres;

    public MovieService(IMovieRepository movies, IGenreRepository genres)
    {
        _movies = movies;
        _genres = genres;
    }

    public async Task<MoviePageResponse> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 1)
            throw new ArgumentException("Page must be at least 1.", nameof(page));

        if (pageSize < 1 || pageSize > MaxPageSize)
            throw new ArgumentException($"Page size must be between 1 and {MaxPageSize}.", nameof(pageSize));

        var (movies, total) = await _movies.GetPageAsync(page, pageSize, cancellationToken);
        var totalPages = total == 0 ? 0 : (int)Math.Ceiling(total / (double)pageSize);

        return new MoviePageResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalResults = total,
            TotalPages = totalPages,
            Results = movies.Select(Map).ToList()
        };
    }

    public async Task<MovieResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var movie = await _movies.GetByIdAsync(id, cancellationToken);
        return movie is null ? null : Map(movie);
    }

    public async Task<MovieResponse> CreateAsync(MovieRequest request, CancellationToken cancellationToken = default)
    {
        var genreIds = DistinctGenreIds(request.GenreIds);
        await EnsureGenresExistAsync(genreIds, cancellationToken);

        var movie = Movie.Create(
            request.Title,
            request.Year,
            request.Overview,
            request.RuntimeMinutes,
            request.PosterUrl,
            request.BackdropUrl,
            CatalogValues.ParseFormat(request.Format),
            CatalogValues.ParseAvailability(request.Availability),
            request.Rating,
            request.IsFeatured);

        foreach (var genreId in genreIds)
            movie.AddGenre(genreId);

        await _movies.AddAsync(movie, cancellationToken);
        await _movies.SaveChangesAsync(cancellationToken);

        var created = await _movies.GetByIdAsync(movie.Id, cancellationToken);
        return Map(created!);
    }

    public async Task<MovieResponse?> UpdateAsync(int id, MovieRequest request, CancellationToken cancellationToken = default)
    {
        var movie = await _movies.GetByIdAsync(id, cancellationToken);
        if (movie is null)
            return null;

        var genreIds = DistinctGenreIds(request.GenreIds);
        await EnsureGenresExistAsync(genreIds, cancellationToken);

        movie.UpdateTitle(request.Title);
        movie.UpdateYear(request.Year);
        movie.UpdateOverview(request.Overview);
        movie.UpdateRuntime(request.RuntimeMinutes);
        movie.UpdatePoster(request.PosterUrl);
        movie.UpdateBackdrop(request.BackdropUrl);
        movie.UpdateRating(request.Rating);
        movie.UpdateFormat(CatalogValues.ParseFormat(request.Format));
        movie.UpdateAvailability(CatalogValues.ParseAvailability(request.Availability));
        movie.SetFeatured(request.IsFeatured);
        SyncGenres(movie, genreIds);

        await _movies.SaveChangesAsync(cancellationToken);
        var updated = await _movies.GetByIdAsync(id, cancellationToken);
        return Map(updated!);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var movie = await _movies.GetByIdAsync(id, cancellationToken);
        if (movie is null)
            return false;

        _movies.Remove(movie);
        await _movies.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task EnsureGenresExistAsync(IReadOnlyCollection<int> genreIds, CancellationToken cancellationToken)
    {
        if (!await _genres.AllExistAsync(genreIds, cancellationToken))
            throw new ArgumentException("One or more genres do not exist.");
    }

    private static List<int> DistinctGenreIds(IEnumerable<int>? genreIds)
    {
        return genreIds?.Distinct().ToList() ?? [];
    }

    private static void SyncGenres(Movie movie, IReadOnlyCollection<int> genreIds)
    {
        var current = movie.MovieGenres.Select(link => link.GenreId).ToHashSet();
        foreach (var genreId in current.Except(genreIds))
            movie.RemoveGenre(genreId);

        foreach (var genreId in genreIds.Except(current))
            movie.AddGenre(genreId);
    }

    internal static MovieResponse Map(Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Rating = movie.Rating,
            Overview = movie.Overview,
            RuntimeMinutes = movie.RuntimeMinutes,
            PosterUrl = movie.PosterUrl,
            BackdropUrl = movie.BackdropUrl,
            Format = movie.Format.ToString(),
            Availability = movie.Availability.ToString(),
            IsFeatured = movie.IsFeatured,
            OriginalTitle = movie.OriginalTitle,
            OriginalLanguage = movie.OriginalLanguage,
            ReleaseDate = movie.ReleaseDate,
            Popularity = movie.Popularity,
            VoteCount = movie.VoteCount,
            Adult = movie.Adult,
            Video = movie.Video,
            PosterPath = movie.PosterPath,
            BackdropPath = movie.BackdropPath,
            ExternalIds = movie.MovieSources
                .OrderBy(link => link.Source?.Name)
                .Select(link => new MovieExternalIdResponse
                {
                    SourceId = link.SourceId,
                    SourceName = link.Source?.Name ?? string.Empty,
                    ExternalId = link.ExternalId
                })
                .ToList(),
            Genres = movie.MovieGenres
                .Where(link => link.Genre is not null)
                .Select(link => new GenreResponse
                {
                    Id = link.Genre.Id,
                    Name = link.Genre.Name,
                    ImageUrl = link.Genre.ImageUrl
                })
                .OrderBy(genre => genre.Name)
                .ToList(),
            Credits = movie.Credits
                .Where(credit => credit.Person is not null)
                .OrderBy(credit => credit.BillingOrder)
                .Select(credit => new MovieCreditResponse
                {
                    PersonId = credit.PersonId,
                    Name = credit.Person.Name,
                    PhotoUrl = credit.Person.PhotoUrl,
                    Role = credit.Role.ToString(),
                    BillingOrder = credit.BillingOrder
                })
                .ToList()
        };
    }
}
