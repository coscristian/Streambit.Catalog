namespace Streambit.Contracts.People;

public class PersonResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string ProfilePath { get; set; } = string.Empty;
    public bool Adult { get; set; }
    public int Gender { get; set; }
    public string KnownForDepartment { get; set; } = string.Empty;
    public double Popularity { get; set; }
    public List<PersonExternalIdResponse> ExternalIds { get; set; } = [];
    public List<PersonKnownForResponse> KnownFor { get; set; } = [];
}

public class PersonExternalIdResponse
{
    public int SourceId { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
}
