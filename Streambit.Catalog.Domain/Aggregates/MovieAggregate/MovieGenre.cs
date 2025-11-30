using Streambit.Catalog.Domain.Aggregates.GenreAggregate;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public static MovieGenre Create(int movieId, int genreId)
        {
            if (movieId <= 0) throw new ArgumentException("MovieId cannot be empty.", nameof(movieId));
            if (genreId <= 0) throw new ArgumentException("GenreId cannot be empty.", nameof(genreId));

            return new MovieGenre
            {
                MovieId = movieId,
                GenreId = genreId
            };
        }
    }
}
