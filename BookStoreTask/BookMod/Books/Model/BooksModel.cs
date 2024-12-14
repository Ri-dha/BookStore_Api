using BookStoreTask.BookMod.Books.utli;
using BookStoreTask.BookMod.Catograzation.Author.model;
using BookStoreTask.BookMod.Catograzation.Genere.Model;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Books.Model;

public class BooksModel:BaseEntity<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ISBN { get; set; }
    public List<ProjectFiles> Images { get; set; } = new List<ProjectFiles>();
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int? Pages { get; set; }
    public int? PublishYear { get; set; }
    public bool AvailabilityStatus { get; set; } = true;
    public LanguageEnum? Language { get; set; }
    public Guid? AuthorId { get; set; }
    public Authors Author { get; set; }
    public Guid? GenreId { get; set; }
    public Genres Genre { get; set; }
    
    
    public void ChangeCheckOut(int quantity)
    {
        if (quantity > Quantity)
        {
            throw new Exception("Not enough stock available");
        }

        Quantity -= quantity;
    }
    
    
}