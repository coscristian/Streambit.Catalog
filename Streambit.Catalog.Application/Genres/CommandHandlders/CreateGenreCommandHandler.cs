using AutoMapper;
using MediatR;
using Streambit.Catalog.Application.Genres.Commands;
using Streambit.Catalog.Contracts.Dto.Genres.Responses;
using Streambit.Catalog.Dal;
using Streambit.Catalog.Domain.Aggregates.GenreAggregate;

namespace Streambit.Catalog.Application.Genres.CommandHandlders;

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, List<GenreCreateResponseDto>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CreateGenreCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<GenreCreateResponseDto>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newGenres = request.Genres
                .Select(newGenre => Genre.Create(newGenre.Name))
                .ToList();
            
            await _context.Genres.AddRangeAsync(newGenres, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<List<GenreCreateResponseDto>>(newGenres);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}