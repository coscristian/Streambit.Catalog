using Streambit.Catalog.Domain.Aggregates.MovieAggregate;

namespace Streambit.Catalog.Domain.Aggregates.LanguageAggregate
{
    public class Language
    {
        public int LanguageId { get; private set; }
        public string Name { get; private set; }
        public string EnglishName { get; private set; }
        public string Iso6391 { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        
        private Language() { }

        // Factory Method
        public static Language CreateLanguage(string name, string englishName, string iso6391)
        {
            if (string.IsNullOrWhiteSpace(englishName))
                throw new ArgumentException("English name cannot be empty.", nameof(englishName));

            if (string.IsNullOrWhiteSpace(name))
                name = englishName;

            if (string.IsNullOrWhiteSpace(iso6391) || iso6391.Length != 2)
                throw new ArgumentException("ISO 639-1 code must have exactly 2 letters.", nameof(iso6391));

            return new Language
            {
                Name = name,
                EnglishName = englishName,
                Iso6391 = iso6391.ToLowerInvariant(),
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }
        
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty.", nameof(newName));

            if (newName == Name)
            {
                return;
            }
            
            Name = newName;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateEnglishName(string newEnglishName)
        {
            if (string.IsNullOrWhiteSpace(newEnglishName))
                throw new ArgumentException("English name cannot be empty.", nameof(newEnglishName));

            if (newEnglishName == EnglishName)
            {
                return;
            }
            
            EnglishName = newEnglishName;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateIso6391(string newIso6391)
        {
            if (string.IsNullOrWhiteSpace(newIso6391) || newIso6391.Length != 2)
                throw new ArgumentException("ISO 639-1 code must have exactly 2 letters.", nameof(newIso6391));

            var normalized = newIso6391.ToLowerInvariant();
            if (normalized == Iso6391)
            {
                return;
            }
            
            Iso6391 = normalized;
            LastModified = DateTime.UtcNow;
        }
    }
}
