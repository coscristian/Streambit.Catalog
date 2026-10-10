namespace Streambit.Contracts.Genres;

public class GenreResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<GenreExternalIdResponse> ExternalIds { get; set; } = [];
}

public class GenreExternalIdResponse
{
    public int SourceId { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
}
