using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.Utli;

namespace BookStoreTask.Cart;

public class CartsDto:BaseDto<Guid>
{
    public decimal TotalPrice { get; set; }
    public Guid CustomerId { get; set; }
    public List<CartItemDto> CartItems { get; set; }
}