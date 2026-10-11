using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Streambit.Application.Movies;

namespace Streambit.Api.Tmdb;

public class TmdbMovieClient : ITmdbMovieClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _http;
    private readonly TmdbOptions _options;
    private readonly SyncRequestLogger _syncLogs;

    public TmdbMovieClient(HttpClient http, IOptions<TmdbOptions> options, SyncRequestLogger syncLogs)
    {
        _http = http;
        _options = options.Value;
        _syncLogs = syncLogs;
    }

    public async Task<TmdbDiscoverMoviesPage> GetPopularMoviesAsync(int page, CancellationToken cancellationToken = default)
    {
        if (page < 1)
            throw new ArgumentException("Page must be at least 1.", nameof(page));

        var response = await GetPageAsync(page, cancellationToken);
        var movies = response.Results
            .Select(MapMovie)
            .ToList();

        return new TmdbDiscoverMoviesPage(response.Page, response.TotalPages, movies);
    }

    private async Task<TmdbDiscoverMoviesResponse> GetPageAsync(int page, CancellationToken cancellationToken)
    {
        const string operation = "discover/movie";
        var requestUri =
            $"{operation}?include_adult=false&include_video=false&language={Uri.EscapeDataString(_options.Language)}&page={page}&sort_by=popularity.desc";
        var requestUrl = BuildRequestUrl(requestUri);

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            var failedAt = DateTime.UtcNow;
            await _syncLogs.WriteAsync(
                operation,
                page,
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

        for (var attempt = 1; attempt <= 5; attempt++)
        {
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
                    operation,
                    page,
                    attempt,
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

                if (httpResponse.StatusCode == HttpStatusCode.TooManyRequests && attempt < 5)
                {
                    var delay = httpResponse.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(2 * attempt);
                    await Task.Delay(delay, cancellationToken);
                    continue;
                }

                if (!succeeded)
                    throw new HttpRequestException(error);

                return JsonSerializer.Deserialize<TmdbDiscoverMoviesResponse>(body, JsonOptions)
                    ?? new TmdbDiscoverMoviesResponse();
            }
            catch (Exception exception) when (!logged)
            {
                watch.Stop();
                await _syncLogs.WriteAsync(
                    operation,
                    page,
                    attempt,
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

        throw new InvalidOperationException("TMDB discover movies request was rate limited.");
    }

    private TmdbMovie MapMovie(TmdbMovieItem item)
    {
        var posterPath = item.PosterPath?.Trim() ?? string.Empty;
        var backdropPath = item.BackdropPath?.Trim() ?? string.Empty;
        return new TmdbMovie(
            item.Id.ToString(),
            item.Title?.Trim() ?? string.Empty,
            item.OriginalTitle?.Trim() ?? string.Empty,
            item.OriginalLanguage?.Trim() ?? string.Empty,
            item.Overview?.Trim() ?? string.Empty,
            item.ReleaseDate?.Trim() ?? string.Empty,
            item.Popularity,
            item.VoteAverage,
            item.VoteCount,
            item.Adult,
            item.Video,
            posterPath,
            BuildImageUrl(posterPath),
            backdropPath,
            BuildImageUrl(backdropPath),
            item.GenreIds ?? []);
    }

    private string BuildImageUrl(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;

        var baseUrl = string.IsNullOrWhiteSpace(_options.ImageBaseUrl)
            ? "https://image.tmdb.org/t/p/w500"
            : _options.ImageBaseUrl.TrimEnd('/');

        return baseUrl + "/" + path.TrimStart('/');
    }

    private string BuildRequestUrl(string requestUri)
    {
        if (_http.BaseAddress is null)
            return requestUri;

        return new Uri(_http.BaseAddress, requestUri).AbsoluteUri;
    }

    private sealed class TmdbDiscoverMoviesResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("results")]
        public List<TmdbMovieItem> Results { get; set; } = [];
    }

    private sealed class TmdbMovieItem
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonPropertyName("genre_ids")]
        public List<int>? GenreIds { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("original_language")]
        public string? OriginalLanguage { get; set; }

        [JsonPropertyName("original_title")]
        public string? OriginalTitle { get; set; }

        [JsonPropertyName("overview")]
        public string? Overview { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }
    }
}
