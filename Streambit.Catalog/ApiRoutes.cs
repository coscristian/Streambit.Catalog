namespace Streambit.Catalog.Api;

public class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public class Movies
    {
        public const string GetById = "{id}";
    }

    public class Languages
    {
        public const string GetById = "{id}";
        public const string CreateLanguages = "CreateLanguages";
    }

    public class Genres
    {
        public const string GetById = "{id}";
        public const string Create = "create";
    }
}