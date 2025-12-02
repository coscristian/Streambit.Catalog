namespace Streambit.Catalog.Contracts.Dto.Languages.Requests;

public class LanguageCreate
{
    public string Name { get; set; } = string.Empty;
    public string EnglishName { get; set; } = string.Empty;
    public string Iso6391 { get; set; } = string.Empty;
}