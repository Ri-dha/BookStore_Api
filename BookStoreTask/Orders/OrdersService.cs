using AutoMapper;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Orders;

public interface IOrdersService
{
    Task<(OrdersDto dto, string? error)> CreateOrder(OrdersCreateForm form);
    Task<(OrdersDto dto, string? error)> AcceptOrder(Guid id);
    Task<(OrdersDto dto, string? error)> ShipOrder(Guid id);
    Task<(OrdersDto dto, string? error)> DeliverOrder(Guid id);
    
    Task<(OrdersDto dto, string? error)> GetOrder(Guid id);
    Task<(List<OrdersDto> dto,int? total ,string? error)> GetOrders(OrderFilterForm form);
}


public class OrdersService:IOrdersService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;


    public OrdersService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<(OrdersDto dto, string? error)> CreateOrder(OrdersCreateForm form)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == form.CustomerId,
            include: source => source.Include(x => x.Cart));
        
        var cart = await _repositoryWrapper.CartRepo.Get(x => x.CustomerId == form.CustomerId,
            include: source => source.Include(x => x.CartItems).ThenInclude(x => x.Book));
        if (cart == null)
        {
            return (null, "Cart not found");
        }
        
        if (customer == null)
        {
            return (null, "Customer not found");
        }
        if (cart.CartItems.Count == 0)
        {
            return (null, "Cart is empty");
        }
        

        foreach (var cartItems in cart.CartItems)
        {
            if (cartItems.Quantity > cartItems.Book.Quantity)
            {
                return (null, "Not enough stock available for: " + cartItems.Book.Title);
            }
        }
        
        var order = new OrdersModel
        {
            CustomerId = form.CustomerId,
            Customer = customer,
            OrderStatus = ShippingStatus.Pending
        };
        order.OrderDate = DateTime.UtcNow;
        var cartItemslist = cart.CartItems.Select(x => new CartItem()
        {
            BookId = x.BookId,
            Book = x.Book,
            Quantity = x.Quantity,
            OrderId = order.Id,
            Order = order
        }).ToList();
        
        order.CartItems = cartItemslist;
        
        
        
        order.TotalPrice = cart.TotalPrice;
        order.Address = form.Address;
        order.PhoneNumber = form.PhoneNumber;
        order.Notes = form.Notes;
        await _repositoryWrapper.OrdersRepoisotry.Add(order);
        cart.ClearCart();
        await _repositoryWrapper.CartRepo.Update(cart, cart.Id);
        var orderDto = _mapper.Map<OrdersDto>(order);
        return (orderDto, null);

    }

    public async Task<(OrdersDto dto, string? error)> AcceptOrder(Guid id)
    {
        var order = await _repositoryWrapper.OrdersRepoisotry.Get(x => x.Id == id,
            include: source => source.Include(x => x.CartItems).ThenInclude(x => x.Book));
        if (order == null)
        {
            return (null, "Order not found");
        }

        if (order.OrderStatus != ShippingStatus.Pending)
        {
            return (null, "Order is not pending");
        }

        order.OrderStatus = ShippingStatus.Approved;
        await _repositoryWrapper.OrdersRepoisotry.Update(order, order.Id);
        var orderDto = _mapper.Map<OrdersDto>(order);
        return (orderDto, null);
    }

    public async Task<(OrdersDto dto, string? error)> ShipOrder(Guid id)
    {
        var order = await _repositoryWrapper.OrdersRepoisotry.Get(x => x.Id == id,
            include: source => source.Include(x => x.CartItems).ThenInclude(x => x.Book));
        if (order == null)
        {
            return (null, "Order not found");
        }

        if (order.OrderStatus != ShippingStatus.Approved)
        {
            return (null, "Order is not approved");
        }

        order.OrderStatus = ShippingStatus.Shipping;
        order.ShippedDate = DateTime.UtcNow;
        await _repositoryWrapper.OrdersRepoisotry.Update(order, order.Id);
        var orderDto = _mapper.Map<OrdersDto>(order);
        return (orderDto, null);
    }

    public async Task<(OrdersDto dto, string? error)> DeliverOrder(Guid id)
    {
        var order = await _repositoryWrapper.OrdersRepoisotry.Get(x => x.Id == id,
            include: source => source.Include(x => x.CartItems).ThenInclude(x => x.Book));
        if (order == null)
        {
            return (null, "Order not found");
        }

        if (order.OrderStatus != ShippingStatus.Shipping)
        {
            return (null, "Order is not shipping");
        }

        foreach (var book in order.CartItems)
        {
            book.Book.Quantity -= book.Quantity;
            if (book.Book.Quantity == 0)
            {
                book.Book.AvailabilityStatus = false;
            }
            
            await _repositoryWrapper.BooksRepository.Update(book.Book, book.Book.Id);
        }

        order.OrderStatus = ShippingStatus.Delivered;
        order.DeliveredDate = DateTime.UtcNow;
        await _repositoryWrapper.OrdersRepoisotry.Update(order, order.Id);
        var orderDto = _mapper.Map<OrdersDto>(order);
        return (orderDto, null);
    }

    public async Task<(OrdersDto dto, string? error)> GetOrder(Guid id)
    {
        var order = await _repositoryWrapper.OrdersRepoisotry.Get(x => x.Id == id,
            include: source => source.Include(x => x.CartItems).ThenInclude(x => x.Book));
        if (order == null)
        {
            return (null, "Order not found");
        }

        var orderDto = _mapper.Map<OrdersDto>(order);
        return (orderDto, null);
    }

    public async Task<(List<OrdersDto> dto,int? total ,string? error)> GetOrders(OrderFilterForm form)
    {
        var (orders, total) = await _repositoryWrapper.OrdersRepoisotry.GetAll(
            x=> ( x.OrderStatus == form.OrderStatus || form.OrderStatus == null) &&
                (x.CustomerId == form.CustomerId || form.CustomerId == null) &&
                (x.PhoneNumber == form.PhoneNumber || form.PhoneNumber == null)
            ,
            include: source => source.Include(x => x.CartItems).ThenInclude(x => x.Book),
            form.PageNumber,
            form.PageSize
                
            );
        
        var orderDtos = orders.Select(x => _mapper.Map<OrdersDto>(x)).ToList();
        return (orderDtos, total, null);
    }
}