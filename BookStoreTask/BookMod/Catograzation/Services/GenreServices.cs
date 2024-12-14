using AutoMapper;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Utli;
using BookStoreTask.BookMod.Catograzation.Dto;
using BookStoreTask.BookMod.Catograzation.Genere.Model;
using BookStoreTask.BookMod.Catograzation.Payload;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Catograzation.Services;

public interface IGenreServices
{
    Task<(GenereDto dto, string? error)> AddGenreAsync(GenereCreateForm form);
    Task<(GenereDto dto, string? error)> UpdateGenreAsync(GenereUpdateForm form, Guid id);
    Task<(GenereDto dto, string? error)> DeleteGenreAsync(Guid id);
    Task<(GenereDto dto, string? error)> GetGenreAsync(Guid id);
    Task<(List<GenereDto> dtos, int? totalCount, string? error)> GetGenresAsync(GenereFilterForm form);
}

public class GenreServices : IGenreServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public GenreServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, IFileService fileService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _fileService = fileService;
    }


    public async Task<(GenereDto dto, string? error)> AddGenreAsync(GenereCreateForm form)
    {
        var genre = _mapper.Map<Genres>(form);
        genre.Type = CategoriesTypes.Genre;

        var genreEntity = await _repositoryWrapper.GernesRepository.Add(genre);

        if (genreEntity == null)
        {
            return (null, "Error in adding genre");
        }


        var genreDto = _mapper.Map<GenereDto>(genreEntity);

        return (genreDto, null);
    }

    public async Task<(GenereDto dto, string? error)> UpdateGenreAsync(GenereUpdateForm form, Guid id)
    {
        var genre = _mapper.Map<Genres>(form);

        var genreEntity = await _repositoryWrapper.GernesRepository.Update(genre, id);

        if (genreEntity == null)
        {
            return (null, "Error in updating genre");
        }

        var genreDto = _mapper.Map<GenereDto>(genreEntity);

        return (genreDto, null);
    }

    public async Task<(GenereDto dto, string? error)> DeleteGenreAsync(Guid id)
    {
        var genreEntity = await _repositoryWrapper.GernesRepository.SoftDelete(id);

        if (genreEntity == null)
        {
            return (null, "Error in deleting genre");
        }

        var genreDto = _mapper.Map<GenereDto>(genreEntity);

        return (genreDto, null);
    }

    public async Task<(GenereDto dto, string? error)> GetGenreAsync(Guid id)
    {
        var genreEntity = await _repositoryWrapper.GernesRepository.Get(x => x.Id == id);

        if (genreEntity == null)
        {
            return (null, "Genre not found");
        }

        var genreDto = _mapper.Map<GenereDto>(genreEntity);

        return (genreDto, null);
    }

    public async Task<(List<GenereDto> dtos, int? totalCount, string? error)> GetGenresAsync(GenereFilterForm form)
    {  
        var (genres, totalCount) = await _repositoryWrapper.GernesRepository.GetAll<GenereDto>(
            x => (x.Name.Contains(form.Name)), form.PageNumber, form.PageSize);
        return (genres, totalCount, null);
    }
}