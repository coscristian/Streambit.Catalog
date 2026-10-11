namespace Streambit.Contracts.Languages.Responses;

public class LanguageResponse
{
    public int LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EnglishName { get; set; } = string.Empty;
    public string Iso6391 { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModified { get; set; }
}
