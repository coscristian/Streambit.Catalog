using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public class MovieGenre
    {
        public Guid MovieGenreId { get; private set; }
        public Guid MovieId { get; private set; }
        public Guid GenreId { get; private set; }
    }
}
