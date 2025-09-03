namespace Streambit.Catalog.Domain.Aggregates.CountryAggregate;

public class Country
{
    public int CountryId { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
}