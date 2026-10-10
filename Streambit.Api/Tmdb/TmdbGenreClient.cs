using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Streambit.Application.Genres;

namespace Streambit.Api.Tmdb;

public class TmdbGenreClient : ITmdbGenreClient
{
    private readonly HttpClient _http;
    private readonly TmdbOptions _options;

    public TmdbGenreClient(HttpClient http, IOptions<TmdbOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public Task<IReadOnlyList<TmdbGenre>> GetMovieGenresAsync(CancellationToken cancellationToken = default)
    {
        return GetGenresAsync("genre/movie/list", cancellationToken);
    }

    public Task<IReadOnlyList<TmdbGenre>> GetTvGenresAsync(CancellationToken cancellationToken = default)
    {
        return GetGenresAsync("genre/tv/list", cancellationToken);
    }

    private async Task<IReadOnlyList<TmdbGenre>> GetGenresAsync(string path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new ArgumentException("TMDB API key is missing.");

        var requestUri = $"{path}?language={Uri.EscapeDataString(_options.Language)}";
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        var httpResponse = await _http.SendAsync(request, cancellationToken);
        httpResponse.EnsureSuccessStatusCode();
        var response = await httpResponse.Content.ReadFromJsonAsync<TmdbGenreListResponse>(cancellationToken);
        if (response?.Genres is null)
            return [];

        return response.Genres
            .Select(genre => new TmdbGenre(genre.Id.ToString(), genre.Name))
            .ToList();
    }

    private sealed class TmdbGenreListResponse
    {
        [JsonPropertyName("genres")]
        public List<TmdbGenreItem> Genres { get; set; } = [];
    }

    private sealed class TmdbGenreItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
