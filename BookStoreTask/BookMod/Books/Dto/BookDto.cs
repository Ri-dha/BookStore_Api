using BookStoreTask.BookMod.Books.utli;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Books.Dto;

public class BookDto : BaseDto<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public List<FilesDto> Images { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? Pages { get; set; }
    public int? PublishYear { get; set; }
    public bool? AvailabilityStatus { get; set; }
    public LanguageEnum? Language { get; set; }
    public Guid? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public Guid? GenreId { get; set; }
    public string? GenreName { get; set; }
}