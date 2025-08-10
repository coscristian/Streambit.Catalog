namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public class MovieGenre
    {
        public Guid MovieGenreId { get; private set; }
        public Guid MovieId { get; private set; }
        public Guid GenreId { get; private set; }
    }
}
