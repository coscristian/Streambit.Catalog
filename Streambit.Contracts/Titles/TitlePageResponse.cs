namespace Streambit.Contracts.Titles;

public class TitlePageResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages { get; set; }
    public List<TitleListItemResponse> Results { get; set; } = [];
}
