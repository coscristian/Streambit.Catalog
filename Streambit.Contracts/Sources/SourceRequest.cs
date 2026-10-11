using System.ComponentModel.DataAnnotations;

namespace Streambit.Contracts.Sources;

public class SourceRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
