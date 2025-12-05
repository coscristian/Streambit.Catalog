using Newtonsoft.Json;

namespace Streambit.Catalog.Contracts.Dto.Genres.Requests;

public class GenreCreateRequestDto
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }
    
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
}