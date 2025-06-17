using System;

namespace Streambit.Catalog.Domain.Aggregates.GenreAggregate
{
    public class Genre
    {
        public Guid GenreId { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }

        private Genre() { }

        private Genre(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name cannot be empty.", nameof(name));

            GenreId = id;
            Name = name;
            CreatedDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public static Genre Create(string name)
        {
            return new Genre(Guid.NewGuid(), name);
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
