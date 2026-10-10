using System.ComponentModel.DataAnnotations;

namespace Streambit.Contracts.Credits;

public class CreditRequest
{
    public int MovieId { get; set; }

    public int PersonId { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;

    public int BillingOrder { get; set; }
}
