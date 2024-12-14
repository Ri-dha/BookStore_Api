using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.Orders;
using BookStoreTask.Utli;

namespace BookStoreTask.Cart.CartItems;

public class CartItem : BaseEntity<Guid>
{
    public Guid? CartId { get; set; } // Foreign key to Cart
    public Carts? Cart { get; set; } // Navigation property
    public Guid BookId { get; set; } // Foreign key to BooksModel
    public BooksModel Book { get; set; } // Navigation property
    public int Quantity { get; set; } // Quantity of the book in the cart
    
    public Guid? OrderId { get; set; } // Foreign key to OrdersModel
    public OrdersModel? Order { get; set; } // Navigation property
}