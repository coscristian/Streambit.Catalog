namespace Streambit.Catalog.Contracts.Languages.Responses;

public class LanguageResponse
{
    public Guid LanguageId { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string Iso6391 { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModified { get; set; }
}