using BookStoreTask.BookMod.Books.utli;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Books.Payloads;

public class BookCreateForm
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ISBN { get; set; }
    public IFormFile[] Images { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int? Pages { get; set; }
    public int? PublishYear { get; set; }
    public LanguageEnum? Language { get; set; }
    public Guid? AuthorId { get; set; }
    public Guid? GenreId { get; set; }
}

public class BookUpdateForm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? Pages { get; set; }
    public int? PublishYear { get; set; }
    public bool? AvailabilityStatus { get; set; }
    public LanguageEnum? Language { get; set; }
    public Guid? AuthorId { get; set; }
    public Guid? GenreId { get; set; }
}

public class BookFilterForm: BaseFilter
{
    public string? Title { get; set; }
    public string? ISBN { get; set; }
    public decimal? StartingPrice { get; set; }
    public decimal? EndingPrice { get; set; }
    public int? StartingQuantity { get; set; }
    public int? EndingQuantity { get; set; }
    public int? PublishYear { get; set; }
    public bool? AvailabilityStatus { get; set; }
    public LanguageEnum? Language { get; set; }
    public string? AuthorName { get; set; }
    public string? GenreName { get; set; }
}