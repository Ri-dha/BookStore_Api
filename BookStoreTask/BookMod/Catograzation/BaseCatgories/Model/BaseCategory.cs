using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Utli;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Catograzation.BaseCatgories.Model;

public class BaseCategory:BaseEntity<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<BooksModel> Books { get; set; } = new List<BooksModel>();
    public CategoriesTypes Type { get; set; }
}