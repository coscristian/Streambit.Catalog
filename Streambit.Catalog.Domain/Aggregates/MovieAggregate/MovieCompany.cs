namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate;

public class MovieCompany
{
    public int MovieId { get; private set; }
    
    public int CompanyId { get; private set; }
    
    public DateTime CreateDate { get; private set; }
    
    public DateTime UpdatedBy { get; private set; }
    
    public Guid CreateBy { get; private set; }
    
    public Guid UpdateBy { get; private set; }
}