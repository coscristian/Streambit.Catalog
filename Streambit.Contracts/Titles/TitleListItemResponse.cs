namespace Streambit.Contracts.Titles;

public class TitleListItemResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Rating { get; set; }
    public string PosterUrl { get; set; } = string.Empty;
    public string Availability { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
}
