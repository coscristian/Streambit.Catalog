namespace Streambit.Catalog.Domain.Aggregates.CompanyAggregate;

public class ProductionCompany
{
    public int ProductionCompanyId { get; private set; }
    public string LogoPath { get; private set; }
    public string Name { get; private set; }
    public string OriginCountry { get; private set; } // TODO: Crear refererencia a tabla
}