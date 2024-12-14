namespace BookStoreTask.Cart;

public class AddBookForm
{
    public Guid BookId { get; set; }
    public Guid CustomerId { get; set; }
    public int Quantity { get; set; }
}

public class CheckoutForm
{
    public Guid CustomerId { get; set; }
}

public class GetCartForm
{
    public Guid CustomerId { get; set; }
}
