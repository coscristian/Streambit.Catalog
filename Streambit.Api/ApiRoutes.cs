namespace Streambit.Api;

public class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public class Titles
    {
        public const string GetById = "{id}";
        public const string Sync = "sync";
    }

    public class Genres
    {
        public const string GetById = "{id}";
        public const string Sync = "sync";
        public const string ExternalId = "{id}/external-ids";
        public const string ExternalIdBySource = "{id}/external-ids/{sourceId}";
    }

    public class Sources
    {
        public const string GetById = "{id}";
    }

    public class People
    {
        public const string GetById = "{id}";
        public const string Sync = "sync";
    }

    public class Credits
    {
        public const string GetById = "{movieId:int}/{personId:int}";
    }

    public class Languages
    {
        public const string GetById = "{id}";
        public const string CreateLanguages = "CreateLanguages";
    }
}
