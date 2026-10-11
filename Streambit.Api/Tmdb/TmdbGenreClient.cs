using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Streambit.Application.Genres;

namespace Streambit.Api.Tmdb;

public class TmdbGenreClient : ITmdbGenreClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _http;
    private readonly TmdbOptions _options;
    private readonly SyncRequestLogger _syncLogs;

    public TmdbGenreClient(HttpClient http, IOptions<TmdbOptions> options, SyncRequestLogger syncLogs)
    {
        _http = http;
        _options = options.Value;
        _syncLogs = syncLogs;
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
        var requestUri = $"{path}?language={Uri.EscapeDataString(_options.Language)}";
        var requestUrl = BuildRequestUrl(requestUri);

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            var failedAt = DateTime.UtcNow;
            await _syncLogs.WriteAsync(
                path,
                page: null,
                attempt: 1,
                HttpMethod.Get.Method,
                requestUrl,
                httpStatusCode: null,
                succeeded: false,
                errorMessage: "TMDB API key is missing.",
                responseBody: null,
                failedAt,
                failedAt,
                cancellationToken);
            throw new ArgumentException("TMDB API key is missing.");
        }

        var startedAt = DateTime.UtcNow;
        var watch = Stopwatch.StartNew();
        var logged = false;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            using var httpResponse = await _http.SendAsync(request, cancellationToken);
            var body = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            watch.Stop();
            var finishedAt = startedAt.Add(watch.Elapsed);
            var succeeded = httpResponse.IsSuccessStatusCode;
            var error = succeeded
                ? null
                : $"TMDB request failed with status {(int)httpResponse.StatusCode}.";

            await _syncLogs.WriteAsync(
                path,
                page: null,
                attempt: 1,
                HttpMethod.Get.Method,
                requestUrl,
                (int)httpResponse.StatusCode,
                succeeded,
                error,
                body,
                startedAt,
                finishedAt,
                cancellationToken);
            logged = true;

            if (!succeeded)
                throw new HttpRequestException(error);

            var response = JsonSerializer.Deserialize<TmdbGenreListResponse>(body, JsonOptions);
            if (response?.Genres is null)
                return [];

            return response.Genres
                .Select(genre => new TmdbGenre(genre.Id.ToString(), genre.Name))
                .ToList();
        }
        catch (Exception exception) when (!logged)
        {
            watch.Stop();
            await _syncLogs.WriteAsync(
                path,
                page: null,
                attempt: 1,
                HttpMethod.Get.Method,
                requestUrl,
                httpStatusCode: null,
                succeeded: false,
                exception.Message,
                responseBody: null,
                startedAt,
                startedAt.Add(watch.Elapsed),
                cancellationToken);
            throw;
        }
    }

    private string BuildRequestUrl(string requestUri)
    {
        if (_http.BaseAddress is null)
            return requestUri;

        return new Uri(_http.BaseAddress, requestUri).AbsoluteUri;
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
