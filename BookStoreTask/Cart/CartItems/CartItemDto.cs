using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.Cart.CartItems;

public class CartItemDto:BaseEntity<Guid>
{
    public Guid BookId { get; set; } // Foreign key to BooksModel
    public string BookTitle { get; set; }
    public string BookAuthor { get; set; }
    public List<FilesDto> BookImage { get; set; }
    public decimal BookPrice { get; set; }
    public int Quantity { get; set; } // Quantity of the book in the cart
}