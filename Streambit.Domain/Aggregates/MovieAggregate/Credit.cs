using Streambit.Domain.Aggregates.PersonAggregate;

namespace Streambit.Domain.Aggregates.MovieAggregate;

public class Credit
{
    public int MovieId { get; private set; }
    public Movie Movie { get; private set; } = null!;

    public int PersonId { get; private set; }
    public Person Person { get; private set; } = null!;

    public CreditRole Role { get; private set; }
    public int BillingOrder { get; private set; }

    private Credit() { }

    public static Credit Create(int personId, CreditRole role, int billingOrder)
    {
        if (personId <= 0)
            throw new ArgumentException("PersonId is required.", nameof(personId));

        if (billingOrder < 0)
            throw new ArgumentException("Billing order cannot be negative.", nameof(billingOrder));

        if (!Enum.IsDefined(role))
            throw new ArgumentException("Role is not valid.", nameof(role));

        return new Credit
        {
            PersonId = personId,
            Role = role,
            BillingOrder = billingOrder
        };
    }

    public void ChangeRole(CreditRole role)
    {
        if (!Enum.IsDefined(role))
            throw new ArgumentException("Role is not valid.", nameof(role));

        Role = role;
    }

    public void ChangeBillingOrder(int billingOrder)
    {
        if (billingOrder < 0)
            throw new ArgumentException("Billing order cannot be negative.", nameof(billingOrder));

        BillingOrder = billingOrder;
    }
}
