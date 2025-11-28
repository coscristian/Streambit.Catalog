using Streambit.Catalog.Domain.Aggregates.CompanyAggregate;
using Streambit.Catalog.Domain.Aggregates.CountryAggregate;
using Streambit.Catalog.Domain.Aggregates.GenreAggregate;
using Streambit.Catalog.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Catalog.Domain.Aggregates.MovieAggregate
{
    public class Movie
    {
        
        // Fields
        private readonly List<MovieGenre> _movieGenres = []; 
        public IEnumerable<MovieGenre> MovieGenres { get { return _movieGenres; } }

        public Guid MovieId { get; private set; }
        public string Title { get; private set; }
        public string OriginalTitle { get; private set; }
        public string Overview { get; private set; }
        public decimal Popularity { get; private set; }
        
        public Guid OriginalLanguageId { get; private set; }
        public virtual Language OriginalLanguage { get; private set; }
        
        public Guid OriginCountryId { get; private set; }
        public virtual Country OriginCountry { get; private set; }
        
        //public IEnumerable<Genre> Genres { get { return _genres; } }
        //public virtual IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();
        
        public ICollection<ProductionCompany> ProductionCompanies { get; private set; }
        
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
        public string BackdropPath { get; private set; }
        public decimal Budget { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        
        
        public IEnumerable<Country> ProductionCountries { get; private set; }
        
        public IEnumerable<Language> SpokenLanguages { get; private set; }

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
                BackdropPath = backdropPath,
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

            OriginalTitle = newOriginalTitle;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateOriginalLanguage(Language newOriginalLanguage)
        {
            if (newOriginalLanguage is null)
            {
                throw new ArgumentNullException(nameof(newOriginalLanguage), "Language cannot be null.");
            }

            if (OriginalLanguage.LanguageId == newOriginalLanguage.LanguageId)
            {
                return;
            }

            OriginalLanguage = newOriginalLanguage;
            LastModified = DateTime.UtcNow;                        
        }

        public void UpdateOverview(string newOverview)
        {
            if (newOverview is null)
            {
                throw new ArgumentNullException(nameof(newOverview), "Overview cannot be null.");
            }

            if (string.Equals(Overview, newOverview))
            {
                return;
            }

            Overview = newOverview;
            LastModified = DateTime.UtcNow;
        }

        public void UpdatePopularity(decimal newPopularity)
        {
            if (newPopularity < 0)
            {
                throw new ArgumentNullException(nameof(newPopularity), "Popularity cannot be negative.");
            }

            if (newPopularity == Popularity)
            {
                return;
            }
            
            Popularity = newPopularity;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateStatus(MovieStatus newStatus)
        {
            if (newStatus == Status)
            {
                return;
            }
            
            Status = newStatus;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateReleaseDate(DateTime newReleaseDate)
        {
            if (newReleaseDate == ReleaseDate)
            {
                return;
            }
            
            ReleaseDate = newReleaseDate;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateRevenue(int newRevenue)
        {
            if (newRevenue < 0)
            {
                throw new ArgumentNullException(nameof(newRevenue), "Revenue cannot be negative.");
            }
            
            Revenue = newRevenue;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateRuntime(int newRuntime)
        {
            if (newRuntime < 0)
            {
                throw new ArgumentNullException(nameof(newRuntime), "Runtime cannot be negative.");
            }
            
            Runtime = newRuntime;
            LastModified = DateTime.UtcNow;
        }
        
        public void UpdateTagLine(string newTagLine)
        {
            if (newTagLine == TagLine)
            {
                return;
            }
        
            TagLine = newTagLine;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateHasVideo(bool hasVideo)
        {
            if (HasVideo == hasVideo)
            {
                return;
            }
            
            HasVideo = hasVideo;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateVoteAverage(decimal newVoteAverage)
        {
            if (newVoteAverage < 0 || newVoteAverage > 10)
            {
                throw new ArgumentException("Vote average must be between 0 and 10.", nameof(newVoteAverage));
            }

            if (newVoteAverage == VoteAverage)
            {
                return;
            }
            
            VoteAverage = newVoteAverage;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateVoteCount(int newVoteCount)
        {
            if (newVoteCount < 0)
            {
                throw new ArgumentException("Vote count cannot be negative.", nameof(newVoteCount));
            }

            if (newVoteCount == VoteCount)
            {
                return;
            }
            
            VoteCount = newVoteCount;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateIsAdult(bool isAdult)
        {
            if (IsAdult == isAdult)
            {
                return;
            }
            
            IsAdult = isAdult;
            LastModified = DateTime.UtcNow;
        }
        
        public void UpdateBudget(decimal newBudget)
        {
            if (newBudget < 0)
                throw new ArgumentException("Budget cannot be negative.", nameof(newBudget));

            if (newBudget == Budget)
            {
                return;
            }
            
            Budget = newBudget;
            LastModified = DateTime.UtcNow;
        }

        public void AddGenre(Genre genre)
        {
            ArgumentNullException.ThrowIfNull(genre, nameof(genre));
            
            if (_movieGenres.Any(g => g.GenreId == genre.GenreId))
                return; 
            
            var movieGenre = MovieGenre.Create(MovieId, genre.GenreId);
            _movieGenres.Add(movieGenre);
            
            LastModified = DateTime.UtcNow;
        }

        public void RemoveGenre(Guid genreId)
        {
            var genre = _movieGenres.FirstOrDefault(g => g.GenreId == genreId);
            if (genre is null) return;

            _movieGenres.Remove(genre);
            LastModified = DateTime.UtcNow;
        }
    }
}
