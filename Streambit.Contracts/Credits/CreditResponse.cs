namespace Streambit.Contracts.Credits;

public class CreditResponse
{
    public int MovieId { get; set; }
    public int PersonId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string PersonName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int BillingOrder { get; set; }
}
