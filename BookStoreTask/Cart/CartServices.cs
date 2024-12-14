using AutoMapper;
using BookStoreTask.Auth;
using BookStoreTask.FilesMod;
using BookStoreTask.Orders;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Cart;

public interface ICartServices
{
    Task<(CartsDto, string)> CreateCart(Guid customerId);
    Task<(CartsDto, string)> AddBookToCart(Guid bookId, Guid customerId, int quantity);
    Task<(CartsDto, string)> EmptyCart(Guid customerId);
    Task<(CartsDto, string)> RemoveBookFromCart(Guid bookId, Guid customerId, int quantity);
    Task<(CartsDto, string)> GetCart(Guid customerId);
    Task<(CartsDto, string)> Checkout(OrdersCreateForm form);
}

public class CartServices : ICartServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IFileService _fileService;
    private readonly IOrdersService _ordersService;

    public CartServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService,
        IFileService fileService, IOrdersService ordersService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _tokenService = tokenService;
        _fileService = fileService;
        _ordersService = ordersService;
    }

    public async Task<(CartsDto, string)> CreateCart(Guid customerId)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == customerId,
            include: source => source.Include(x => x.Cart));

        var cart = new Carts
        {
            CustomerId = customerId,
            Customer = customer
        };

        var cartEntity = await _repositoryWrapper.CartRepo.Add(cart);
        if (cartEntity == null)
        {
            return (null, "Error in adding cart");
        }

        customer.Cart = cartEntity;
        customer.CartId = cartEntity.Id;

        await _repositoryWrapper.CustomerRepository.Update(customer, customerId);
        var cartDto = _mapper.Map<CartsDto>(cartEntity);
        return (cartDto, null);
    }

    public async Task<(CartsDto, string)> AddBookToCart(Guid bookId, Guid customerId, int quantity)
    {
        var book = await _repositoryWrapper.BooksRepository.Get(x => x.Id == bookId);
        if (book == null)
        {
            return (null, "Book not found");
        }

        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == customerId,
            include: source => source.Include(x => x.Cart).ThenInclude(x => x.CartItems));
        if (customer.Cart == null)
        {
            return (null, "Cart not found");
        }

        var cart = customer.Cart;
        cart.AddBook(book, quantity);
        await _repositoryWrapper.CartRepo.Update(cart, cart.Id);

        var cartDto = _mapper.Map<CartsDto>(cart);
        return (cartDto, null);
    }

    public async Task<(CartsDto, string)> EmptyCart(Guid customerId)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == customerId,
            include: source => source.Include(x => x.Cart).ThenInclude(x => x.CartItems));

        if (customer.Cart == null)
        {
            return (null, "Cart not found");
        }

        var cart = customer.Cart;
        cart.ClearCart();
        await _repositoryWrapper.CartRepo.Update(cart, cart.Id);

        var cartDto = _mapper.Map<CartsDto>(cart);
        return (cartDto, null);
    }

    public async Task<(CartsDto, string)> RemoveBookFromCart(Guid bookId, Guid customerId, int quantity)
    {
        var book = await _repositoryWrapper.BooksRepository.Get(x => x.Id == bookId);
        if (book == null)
        {
            return (null, "Book not found");
        }

        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == customerId,
            include: source => source.Include(x => x.Cart).ThenInclude(x => x.CartItems));

        if (customer.Cart == null)
        {
            return (null, "Cart not found");
        }

        var cart = customer.Cart;
        cart.RemoveBook(book.Id, quantity);
        await _repositoryWrapper.CartRepo.Update(cart, cart.Id);
        var cartDto = _mapper.Map<CartsDto>(cart);
        return (cartDto, null);
    }

    public async Task<(CartsDto, string)> GetCart(Guid customerId)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == customerId,
            include: source => source.Include(x => x.Cart)
                .ThenInclude(x => x.CartItems)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .ThenInclude(x => x.Books)
                .ThenInclude(x => x.Images)
            
               
            );

        if (customer.Cart == null)
        {
            return (null, "Cart not found");
        }

        var cart = customer.Cart;
        var cartDto = _mapper.Map<CartsDto>(cart);
        return (cartDto, null);
    }

    public async Task<(CartsDto, string)> Checkout(OrdersCreateForm form)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == form.CustomerId,
            include: source => source.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Book));

        if (customer.Cart == null)
        {
            return (null, "Cart not found");
        }

        var cart = customer.Cart;
        if (cart.CartItems.Count == 0)
        {
            return (null, "Cart is empty");
        }

        var order = await _ordersService.CreateOrder(form);
        if (order.error != null)
        {
            return (null, order.error);
        }
        await _repositoryWrapper.CartRepo.Update(cart, cart.Id);
        var cartDto = _mapper.Map<CartsDto>(cart);
        return (cartDto, null);
    }
}