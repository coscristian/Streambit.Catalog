using System.ComponentModel.DataAnnotations;

namespace Streambit.Contracts.Genres;

public class GenreExternalIdRequest
{
    public int SourceId { get; set; }

    [Required]
    public string ExternalId { get; set; } = string.Empty;
}
