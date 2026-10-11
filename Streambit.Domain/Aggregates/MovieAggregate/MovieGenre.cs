using Streambit.Domain.Aggregates.GenreAggregate;

namespace Streambit.Domain.Aggregates.MovieAggregate;

public class MovieGenre
{
    public int MovieId { get; private set; }
    public Movie Movie { get; private set; } = null!;

    public int GenreId { get; private set; }
    public Genre Genre { get; private set; } = null!;

    private MovieGenre() { }

    public static MovieGenre Create(int genreId)
    {
        if (genreId <= 0)
            throw new ArgumentException("GenreId is required.", nameof(genreId));

        return new MovieGenre
        {
            GenreId = genreId
        };
    }
}
