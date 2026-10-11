using System.ComponentModel.DataAnnotations;

namespace Streambit.Contracts.Genres;

public class GenreRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;
}
