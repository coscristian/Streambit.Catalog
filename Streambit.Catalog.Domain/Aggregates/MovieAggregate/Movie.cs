using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streambit.Catalog.Domain.Aggregates.GenreAggregate;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public class Movie
    {
        public Guid MovieId { get; private set; }
        public string Title { get; private set; }
        public Language OriginalLanguage { get; private set; }
        public string OriginalTitle { get; private set; }
        public string Overview { get; private set; }
        public decimal Popularity { get; private set; }

        private readonly List<Genre> _genres = [];
        public IEnumerable<Genre> Genres { get; private set; }
        /* Entities to create
        public IEnumerable<Companie> ProductionCompanies { get; private set; }

        public IEnumerable<Countrie> ProductionCountries {get; private set; }

        public IEnumerable<Language> SpokenLanguages {get; private set; }

        */

        // To Check
        public MovieStatus Status { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public int Revenue { get; private set; }
        public int Runtime { get; private set; }
        public string TagLine { get; private set; }
        public bool HasVideo { get; private set; }
        public decimal VoteAverage { get; private set; }
        public int VoteCount { get; private set; }
        public bool IsAdult { get; private set; }
        public string BackdroPath { get; private set; }
        public decimal Budget { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }

        // Factories
        public static Movie CreateMovie(string title, Language originalLanguage,
            string originalTitle, string overview, decimal popularity,
            MovieStatus status, DateTime releaseDate, int revenue, int runtime,
            string tagLine, bool hasVideo, decimal voteAverage, int voteCount,
            bool isAdult, string backdropPath, decimal budget)
        {
            return new Movie
            {
                Title = title,
                OriginalLanguage = originalLanguage,
                OriginalTitle = originalTitle,
                Overview = overview,
                Popularity = popularity,
                Status = status,
                ReleaseDate = releaseDate,
                Revenue = revenue,
                Runtime = runtime,
                TagLine = tagLine,
                HasVideo = hasVideo,
                VoteAverage = voteAverage,
                VoteCount = voteCount,
                IsAdult = isAdult,
                BackdroPath = backdropPath,
                Budget = budget,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty.", nameof(newTitle));

            if (newTitle == Title)
                return;

            Title = newTitle;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateOriginalTitle(string newOriginalTitle)
        {
            if (string.IsNullOrWhiteSpace(newOriginalTitle))
                throw new ArgumentException("Original title cannot be empty.", nameof(newOriginalTitle));

            if (newOriginalTitle == OriginalTitle)
                return;

            Title = newOriginalTitle;
            LastModified = DateTime.UtcNow;
        }

        public void AddGenre(Genre genre)
        {
            _genres.Add(genre);
        }
    }
}
