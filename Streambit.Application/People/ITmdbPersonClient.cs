namespace Streambit.Application.People;

public interface ITmdbPersonClient
{
    Task<TmdbPopularPeoplePage> GetPopularPeopleAsync(int page, CancellationToken cancellationToken = default);
}
