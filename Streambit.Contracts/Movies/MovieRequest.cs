using System.ComponentModel.DataAnnotations;

namespace Streambit.Contracts.Movies;

public class MovieRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal Rating { get; set; }

    public string Overview { get; set; } = string.Empty;

    public int RuntimeMinutes { get; set; }

    [Required]
    public string PosterUrl { get; set; } = string.Empty;

    [Required]
    public string BackdropUrl { get; set; } = string.Empty;

    [Required]
    public string Format { get; set; } = string.Empty;

    [Required]
    public string Availability { get; set; } = string.Empty;

    public bool IsFeatured { get; set; }

    public List<int> GenreIds { get; set; } = [];
}
