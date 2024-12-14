using AutoMapper;
using BookStoreTask.Auth;
using BookStoreTask.BookMod.Catograzation.Author.model;
using BookStoreTask.BookMod.Catograzation.Author.utli;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Utli;
using BookStoreTask.BookMod.Catograzation.Dto;
using BookStoreTask.BookMod.Catograzation.Payload;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Catograzation.Services;

public interface IAuthorServices
{
    Task<(AuthorDto dto, string? error)> AddAuthorAsync(AuthorCreateForm form);
    Task<(AuthorDto dto, string? error)> UpdateAuthorAsync(AuthorUpdateForm form, Guid id);
    Task<(AuthorDto dto, string? error)> DeleteAuthorAsync(Guid id);
    Task<(AuthorDto dto, string? error)> GetAuthorAsync(Guid id);
    Task<(List<AuthorDto> dtos, int? totalCount, string? error)> GetAuthorsAsync(AuthorFilterForm form);
    
    Task<(List<object>? Countries, string? error)> GetCountriesAsync();
    
    
}

public class AuthorServices : IAuthorServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IFileService _fileService;

    public AuthorServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService,
        IFileService fileService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _tokenService = tokenService;
        _fileService = fileService;
    }

    public async Task<(AuthorDto dto, string? error)> AddAuthorAsync(AuthorCreateForm form)
    {
        var author = _mapper.Map<Authors>(form);
        if (form.Image != null)
        {
            var image = await _fileService.SaveFileAsync(form.Image);

            if (image == null)
            {
                return (null, "Error in saving image");
            }

            author.Image = image;
        }
        
        author.Type = CategoriesTypes.Author;

        var authorEntity = await _repositoryWrapper.AuthorsRepository.Add(author);
        if (authorEntity == null)
        {
            return (null, "Error in adding author");
        }

        var authorDto = _mapper.Map<AuthorDto>(authorEntity);
        return (authorDto, null);
    }

    public async Task<(AuthorDto dto, string? error)> UpdateAuthorAsync(AuthorUpdateForm form, Guid id)
    {
        var author = await _repositoryWrapper.AuthorsRepository.Get(x => x.Id == id);

        if (author == null)
        {
            return (null, "Author not found");
        }

        _mapper.Map(form, author);

        if (form.Image != null)
        {
            var image = await _fileService.SaveFileAsync(form.Image);

            if (image == null)
            {
                return (null, "Error in saving image");
            }

            author.Image = image;
        }

        var authorEntity = await _repositoryWrapper.AuthorsRepository.Update(author, id);
        if (authorEntity == null)
        {
            return (null, "Error in updating author");
        }

        var authorDto = _mapper.Map<AuthorDto>(authorEntity);
        return (authorDto, null);
    }

    public async Task<(AuthorDto dto, string? error)> DeleteAuthorAsync(Guid id)
    {
        var author = await _repositoryWrapper.AuthorsRepository.Get(x => x.Id == id);

        if (author == null)
        {
            return (null, "Author not found");
        }

        var authorEntity = await _repositoryWrapper.AuthorsRepository.SoftDelete(id);
        if (authorEntity == null)
        {
            return (null, "Error in deleting author");
        }

        var authorDto = _mapper.Map<AuthorDto>(authorEntity);
        return (authorDto, null);
    }

    public async Task<(AuthorDto dto, string? error)> GetAuthorAsync(Guid id)
    {
        var author = await _repositoryWrapper.AuthorsRepository.Get(x => x.Id == id);

        if (author == null)
        {
            return (null, "Author not found");
        }

        var authorDto = _mapper.Map<AuthorDto>(author);
        return (authorDto, null);
    }

    public async Task<(List<AuthorDto> dtos, int? totalCount, string? error)> GetAuthorsAsync(AuthorFilterForm form)
    {
        var (authors, totalCount) = await _repositoryWrapper.AuthorsRepository.GetAll<AuthorDto>(
            x => (string.IsNullOrEmpty(form.Name) || x.Name.Contains(form.Name)) &&
                 (form.Country == null || x.Country == form.Country) &&
                 x.Deleted == false,
            form.PageNumber,
            form.PageSize
        );

        return (authors, totalCount, null);
    }

    public async Task<(List<object>? Countries, string? error)> GetCountriesAsync()
    {
        var c=Enum.GetValues<CountryEnum>()
            .Cast<CountryEnum>()
            .Select(role => new { Name = role.ToString(), Value = (int)role })
            .ToList<object>();
        
        return (c, null);
        
    }
}