namespace Streambit.Contracts.Movies;

public class MoviePageResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages { get; set; }
    public List<MovieResponse> Results { get; set; } = [];
}
