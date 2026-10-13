namespace Streambit.Contracts.Titles;

public class TitleListRequest
{
    public string? Format { get; set; } = "all";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string OrderBy { get; set; } = "popularity";
    public bool SortAscending { get; set; } = false;
    public int? GenreId { get; set; }
}
