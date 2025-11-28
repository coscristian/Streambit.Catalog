using Streambit.Catalog.Domain.Aggregates.GenreAggregate;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public class MovieGenre
    {
        public Guid MovieGenreId { get; private set; }
        public Guid MovieId { get; private set; }
        public Guid GenreId { get; private set; }
        
        // Navigation properties
        public Movie Movie { get; private set; }
        public Genre Genre { get; private set; }

        public static MovieGenre Create(Guid movieId, Guid genreId)
        {
            if (movieId == Guid.Empty) throw new ArgumentException("MovieId cannot be empty.", nameof(movieId));
            if (genreId == Guid.Empty) throw new ArgumentException("GenreId cannot be empty.", nameof(genreId));

            return new MovieGenre
            {
                MovieId = movieId,
                GenreId = genreId
            };
        }
    }
}
