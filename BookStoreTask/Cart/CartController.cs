using BookStoreTask.Orders;
using BookStoreTask.Utli;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.Cart;

[Route("cart/")]
public class CartController : BaseController
{
    private readonly ICartServices _cartServices;

    public CartController(ICartServices cartServices)
    {
        _cartServices = cartServices;
    }


    [HttpPost("add-book-to-cart")]
    public async Task<IActionResult> AddBookToCart([FromBody] AddBookForm form)
    {
        var (cartDto, error) = await _cartServices.AddBookToCart(form.BookId, form.CustomerId, form.Quantity);
        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(cartDto);
    }


    [HttpPut("empty-cart")]
    public async Task<IActionResult> EmptyCart([FromBody] Guid customerId)
    {
        var (cartDto, error) = await _cartServices.EmptyCart(customerId);
        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(cartDto);
    }


    [HttpPut("remove-book-from-cart")]
    public async Task<IActionResult> RemoveBookFromCart([FromBody] AddBookForm form)
    {
        var (cartDto, error) = await _cartServices.RemoveBookFromCart(form.BookId, form.CustomerId, form.Quantity);
        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(cartDto);
    }


    [HttpGet("get-cart/{customerId}")]
    public async Task<IActionResult> GetCart([FromRoute] Guid customerId)
    {
        var (cartDto, error) = await _cartServices.GetCart(customerId);
        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(cartDto);
    }


    [HttpPut("checkout-cart")]
    public async Task<IActionResult> UpdateCart([FromBody] OrdersCreateForm form)
    {
        var (cartDto, error) = await _cartServices.Checkout(form);
        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(cartDto);
    }
}