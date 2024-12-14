using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.Users.Customers;
using BookStoreTask.Utli;

namespace BookStoreTask.Cart;

public class Carts:BaseEntity<Guid>
{
    public decimal TotalPrice { get; set; }=0;
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    
    
    public void AddBook(BooksModel book, int quantity)
    {
        if (quantity > book.Quantity)
        {
            throw new Exception("Not enough stock available");
        }

        var cartItem = CartItems.FirstOrDefault(ci => ci.BookId == book.Id);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            CartItems.Add(new CartItem
            {
                Book = book,
                Quantity = quantity,
                CartId = this.Id,
                BookId = book.Id
            });
        }

        TotalPrice += book.Price * quantity;
    }

    public void RemoveBook(Guid bookId, int quantity)
    {
        var cartItem = CartItems.FirstOrDefault(ci => ci.BookId == bookId);
        if (cartItem == null)
        {
            throw new Exception("Book not found in cart");
        }

        if (quantity >= cartItem.Quantity)
        {
            CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity -= quantity;
        }

        TotalPrice -= cartItem.Book.Price * quantity;
    }

    public void ClearCart()
    {
        CartItems.Clear();
        TotalPrice = 0;
    }

}
