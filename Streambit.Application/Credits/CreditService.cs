using Streambit.Application.Abstractions;
using Streambit.Contracts.Credits;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Application.Credits;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _credits;
    private readonly IMovieRepository _movies;
    private readonly IPersonRepository _people;

    public CreditService(ICreditRepository credits, IMovieRepository movies, IPersonRepository people)
    {
        _credits = credits;
        _movies = movies;
        _people = people;
    }

    public async Task<IReadOnlyList<CreditResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var credits = await _credits.GetAllAsync(cancellationToken);
        return credits.Select(Map).ToList();
    }

    public async Task<CreditResponse?> GetAsync(int movieId, int personId, CancellationToken cancellationToken = default)
    {
        var credit = await _credits.GetAsync(movieId, personId, cancellationToken);
        return credit is null ? null : Map(credit);
    }

    public async Task<CreditResponse> CreateAsync(CreditRequest request, CancellationToken cancellationToken = default)
    {
        var movie = await _movies.GetByIdAsync(request.MovieId, cancellationToken)
            ?? throw new KeyNotFoundException("Movie was not found.");
        var person = await _people.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new KeyNotFoundException("Person was not found.");

        if (movie.Credits.Any(credit => credit.PersonId == person.Id))
            throw new ArgumentException("This person is already credited on the title.");

        movie.AddCredit(person.Id, CatalogValues.ParseRole(request.Role), request.BillingOrder);
        await _movies.SaveChangesAsync(cancellationToken);

        var created = await _credits.GetAsync(movie.Id, person.Id, cancellationToken);
        return Map(created!);
    }

    public async Task<CreditResponse?> UpdateAsync(int movieId, int personId, CreditRequest request, CancellationToken cancellationToken = default)
    {
        var credit = await _credits.GetAsync(movieId, personId, cancellationToken);
        if (credit is null)
            return null;

        credit.ChangeRole(CatalogValues.ParseRole(request.Role));
        credit.ChangeBillingOrder(request.BillingOrder);
        await _credits.SaveChangesAsync(cancellationToken);
        return Map(credit);
    }

    public async Task<bool> DeleteAsync(int movieId, int personId, CancellationToken cancellationToken = default)
    {
        var credit = await _credits.GetAsync(movieId, personId, cancellationToken);
        if (credit is null)
            return false;

        _credits.Remove(credit);
        await _credits.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static CreditResponse Map(Credit credit)
    {
        return new CreditResponse
        {
            MovieId = credit.MovieId,
            PersonId = credit.PersonId,
            MovieTitle = credit.Movie?.Title ?? string.Empty,
            PersonName = credit.Person?.Name ?? string.Empty,
            PhotoUrl = credit.Person?.PhotoUrl ?? string.Empty,
            Role = credit.Role.ToString(),
            BillingOrder = credit.BillingOrder
        };
    }
}
