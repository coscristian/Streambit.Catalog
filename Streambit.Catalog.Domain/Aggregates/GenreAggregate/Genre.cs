using System;
using System.ComponentModel.DataAnnotations;
using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Domain.Aggregates.GenreAggregate
{
    public class Genre
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        
        public IEnumerable<MovieGenre> MovieGenres => _movieGenres;
        private List<MovieGenre> _movieGenres = []; 
        
        private Genre() { }

        private Genre(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name cannot be empty.", nameof(name));

            Name = name;
            CreatedDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public static Genre Create(string name)
        {
            return new Genre(name);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name cannot be empty.", nameof(name));

            if (name == Name)
                return;

            Name = name;
            LastModified = DateTime.UtcNow;
        }
    }
}
