using Streambit.Application.Abstractions;
using Streambit.Contracts.Genres;
using Streambit.Domain.Aggregates.GenreAggregate;

namespace Streambit.Application.Genres;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genres;
    private readonly ISourceRepository _sources;

    public GenreService(IGenreRepository genres, ISourceRepository sources)
    {
        _genres = genres;
        _sources = sources;
    }

    public async Task<IReadOnlyList<GenreResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var genres = await _genres.GetAllAsync(cancellationToken);
        return genres.Select(Map).ToList();
    }

    public async Task<GenreResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _genres.GetByIdAsync(id, cancellationToken);
        return genre is null ? null : Map(genre);
    }

    public async Task<GenreResponse> CreateAsync(GenreRequest request, CancellationToken cancellationToken = default)
    {
        var genre = Genre.Create(request.Name, request.ImageUrl);
        await _genres.AddAsync(genre, cancellationToken);
        await _genres.SaveChangesAsync(cancellationToken);
        return Map(genre);
    }

    public async Task<GenreResponse?> UpdateAsync(int id, GenreRequest request, CancellationToken cancellationToken = default)
    {
        var genre = await _genres.GetByIdAsync(id, cancellationToken);
        if (genre is null)
            return null;

        genre.UpdateName(request.Name);
        genre.UpdateImage(request.ImageUrl);
        await _genres.SaveChangesAsync(cancellationToken);
        return Map(genre);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _genres.GetByIdAsync(id, cancellationToken);
        if (genre is null)
            return false;

        _genres.Remove(genre);
        await _genres.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<GenreResponse?> AssignExternalIdAsync(int id, GenreExternalIdRequest request, CancellationToken cancellationToken = default)
    {
        var genre = await _genres.GetByIdAsync(id, cancellationToken);
        if (genre is null)
            return null;

        var source = await _sources.GetByIdAsync(request.SourceId, cancellationToken)
            ?? throw new KeyNotFoundException("Source was not found.");

        if (await _genres.ExternalIdInUseAsync(source.Id, request.ExternalId.Trim(), genre.Id, cancellationToken))
            throw new ArgumentException("This external id is already linked to another genre for that source.");

        genre.AssignExternalId(source.Id, request.ExternalId);
        await _genres.SaveChangesAsync(cancellationToken);

        var updated = await _genres.GetByIdAsync(id, cancellationToken);
        return Map(updated!);
    }

    public async Task<GenreResponse?> RemoveExternalIdAsync(int id, int sourceId, CancellationToken cancellationToken = default)
    {
        var genre = await _genres.GetByIdAsync(id, cancellationToken);
        if (genre is null)
            return null;

        genre.RemoveExternalId(sourceId);
        await _genres.SaveChangesAsync(cancellationToken);
        return Map(genre);
    }

    private static GenreResponse Map(Genre genre)
    {
        return new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name,
            ImageUrl = genre.ImageUrl,
            ExternalIds = genre.GenreSources
                .OrderBy(link => link.Source?.Name)
                .Select(link => new GenreExternalIdResponse
                {
                    SourceId = link.SourceId,
                    SourceName = link.Source?.Name ?? string.Empty,
                    ExternalId = link.ExternalId
                })
                .ToList()
        };
    }
}
