using Streambit.Application.Abstractions;
using Streambit.Contracts.Dto.Languages.Requests;
using Streambit.Contracts.Languages.Responses;
using Streambit.Domain.Aggregates.LanguageAggregate;

namespace Streambit.Application.Languages;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languages;

    public LanguageService(ILanguageRepository languages)
    {
        _languages = languages;
    }

    public async Task<IReadOnlyList<LanguageResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var languages = await _languages.GetAllAsync(cancellationToken);
        return languages.Select(Map).ToList();
    }

    public async Task<LanguageResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var language = await _languages.GetByIdAsync(id, cancellationToken);
        return language is null ? null : Map(language);
    }

    public async Task<IReadOnlyList<LanguageResponse>> CreateAsync(IReadOnlyList<LanguageCreate> requests, CancellationToken cancellationToken = default)
    {
        var languages = requests
            .Select(request => Language.CreateLanguage(request.Name, request.EnglishName, request.Iso6391))
            .ToList();

        await _languages.AddRangeAsync(languages, cancellationToken);
        await _languages.SaveChangesAsync(cancellationToken);
        return languages.Select(Map).ToList();
    }

    private static LanguageResponse Map(Language language)
    {
        return new LanguageResponse
        {
            LanguageId = language.LanguageId,
            Name = language.Name,
            EnglishName = language.EnglishName,
            Iso6391 = language.Iso6391,
            CreatedDate = language.CreatedDate,
            LastModified = language.LastModified
        };
    }
}
