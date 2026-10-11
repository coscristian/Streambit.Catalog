using System.ComponentModel.DataAnnotations;

namespace Streambit.Contracts.People;

public class PersonRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PhotoUrl { get; set; } = string.Empty;
}
