using System;
using System.ComponentModel.DataAnnotations;
using Streambit.Domain.Aggregates.MovieAggregate;

namespace Streambit.Domain.Aggregates.GenreAggregate
{
    public class Genre
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        
        public IEnumerable<MovieGenre> MovieGenres => _movieGenres;
        private List<MovieGenre> _movieGenres = [];

        private readonly List<GenreSource> _genreSources = [];
        public IReadOnlyCollection<GenreSource> GenreSources => _genreSources; 
        
        private Genre() { }

        private Genre(string name, string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name cannot be empty.", nameof(name));

            Name = name.Trim();
            ImageUrl = imageUrl?.Trim() ?? string.Empty;
            CreatedDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public static Genre Create(string name, string? imageUrl = null)
        {
            return new Genre(name, imageUrl);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name cannot be empty.", nameof(name));

            if (name == Name)
                return;

            Name = name.Trim();
            LastModified = DateTime.UtcNow;
        }

        public void UpdateImage(string? imageUrl)
        {
            var value = imageUrl?.Trim() ?? string.Empty;
            if (value == ImageUrl)
                return;

            ImageUrl = value;
            LastModified = DateTime.UtcNow;
        }

        public void AssignExternalId(int sourceId, string externalId)
        {
            var existing = _genreSources.FirstOrDefault(link => link.SourceId == sourceId);
            if (existing is null)
            {
                _genreSources.Add(GenreSource.Create(sourceId, externalId));
                return;
            }

            existing.ChangeExternalId(externalId);
        }

        public void RemoveExternalId(int sourceId)
        {
            var existing = _genreSources.FirstOrDefault(link => link.SourceId == sourceId);
            if (existing is null)
                return;

            _genreSources.Remove(existing);
        }
    }
}
