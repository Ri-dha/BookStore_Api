using AutoMapper;
using BookStoreTask.Auth;
using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.BookMod.Books.Payloads;
using BookStoreTask.BookMod.Books.utli;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Books.services;

public interface IBooksServices
{
    Task<(BookDto dto, string? error)> AddBookAsync(BookCreateForm form);
    Task<(BookDto dto, string? error)> UpdateBookAsync(BookUpdateForm form, Guid id);
    Task<(BookDto dto, string? error)> DeleteBookAsync(Guid id);
    Task<(BookDto dto, string? error)> GetBookAsync(Guid id);
    Task<(List<BookDto> dtos, int? totalCount, string? error)> GetBooksAsync(BookFilterForm form);

    Task<(List<object>? Languages, string? error)> GetLanguagesAsync();
}

public class BooksServices : IBooksServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IFileService _fileService;


    public BooksServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService,
        IFileService fileService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _tokenService = tokenService;
        _fileService = fileService;
    }

    public async Task<(BookDto dto, string? error)> AddBookAsync(BookCreateForm form)
    {
        var book = _mapper.Map<BooksModel>(form);
        if (form.Images != null)
        {
            var images = await _fileService.SaveFilesAsync(form.Images);

            if (images == null)
            {
                return (null, "Error in saving images");
            }

            book.Images = images;
        }

        var bookEntity = await _repositoryWrapper.BooksRepository.Add(book);
        if (bookEntity == null)
        {
            return (null, "Error in adding book");
        }

        var bookDto = _mapper.Map<BookDto>(bookEntity);

        return (bookDto, null);
    }

    public async Task<(BookDto dto, string? error)> UpdateBookAsync(BookUpdateForm form, Guid id)
    {
        var book = _mapper.Map<BooksModel>(form);
        var bookEntity = await _repositoryWrapper.BooksRepository.Update(book, id);
        if (bookEntity == null)
        {
            return (null, "Error in updating book");
        }

        var bookDto = _mapper.Map<BookDto>(bookEntity);

        return (bookDto, null);
    }

    public async Task<(BookDto dto, string? error)> DeleteBookAsync(Guid id)
    {
        var bookEntity = await _repositoryWrapper.BooksRepository.SoftDelete(id);
        if (bookEntity == null)
        {
            return (null, "Error in deleting book");
        }

        var bookDto = _mapper.Map<BookDto>(bookEntity);

        return (bookDto, null);
    }

    public async Task<(BookDto dto, string? error)> GetBookAsync(Guid id)
    {
        var bookEntity = await _repositoryWrapper.BooksRepository.Get(x => x.Id == id);
        if (bookEntity == null)
        {
            return (null, "Book not found");
        }

        var bookDto = _mapper.Map<BookDto>(bookEntity);

        return (bookDto, null);
    }

    public async Task<(List<BookDto> dtos, int? totalCount, string? error)> GetBooksAsync(BookFilterForm form)
    {
        var (books, totalCount) = await _repositoryWrapper.BooksRepository.GetAll<BookDto>(
            x => (string.IsNullOrEmpty(form.Title) || x.Title.Contains(form.Title)) &&
                 (string.IsNullOrEmpty(form.AuthorName) || x.Author.Name == form.AuthorName) &&
                 (string.IsNullOrEmpty(form.GenreName) || x.Genre.Name == form.GenreName) &&
                 (form.Language == null || x.Language == form.Language) &&
                 (form.StartingPrice == null || x.Price >= form.StartingPrice) &&
                 (form.EndingPrice == null || x.Price <= form.EndingPrice) &&
                 (form.StartingQuantity == null || x.Quantity >= form.StartingQuantity) &&
                 (form.EndingQuantity == null || x.Quantity <= form.EndingQuantity) &&
                 (form.PublishYear == null || x.PublishYear == form.PublishYear) &&
                 (form.AvailabilityStatus == null || x.AvailabilityStatus == form.AvailabilityStatus) &&
                 x.Deleted == false
            ,
            form.PageNumber, form.PageSize);
        return (books, totalCount, null);
    }

    public async Task<(List<object>? Languages, string? error)> GetLanguagesAsync()
    {
        var languages = Enum.GetValues<LanguageEnum>()
            .Cast<LanguageEnum>()
            .Select(role => new { Name = role.ToString(), Value = (int)role })
            .ToList<object>();

        return (languages, null);
    }
}