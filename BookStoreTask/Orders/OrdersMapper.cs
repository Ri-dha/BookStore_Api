using AutoMapper;
using BookStoreTask.Cart.CartItems;

namespace BookStoreTask.Orders;

public class OrdersMapper : Profile
{
    public OrdersMapper()
    {
        // Map OrdersModel to OrdersDto
        CreateMap<OrdersModel, OrdersDto>()
            .ForMember(dest => dest.CustomerName, 
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Username : null))
            .ForMember(dest => dest.CustomerEmail, 
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Email : null))
            .ForMember(dest => dest.CartItems, 
                opt => opt.MapFrom(src => src.CartItems));

        // // Map OrdersDto to OrdersModel (for updating or creating orders)
        // CreateMap<OrdersDto, OrdersModel>()
        //     .ForMember(dest => dest.Customer, 
        //         opt => opt.Ignore()) // Customer will be resolved separately
        //     .ForMember(dest => dest.CartItems, 
        //         opt => opt.Ignore()); // CartItems will be handled separately
        //
        // // Map CartItem to CartItemDto
        // CreateMap<CartItem, CartItemDto>()
        //     .ForMember(dest => dest.BookTitle, 
        //         opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
        //     .ForMember(dest => dest.BookPrice, 
        //         opt => opt.MapFrom(src => src.Book != null ? src.Book.Price : 0));
        //
        // // Map CartItemDto to CartItem (for updating or creating cart items)
        // CreateMap<CartItemDto, CartItem>()
        //     .ForMember(dest => dest.Book, 
        //         opt => opt.Ignore()) // Book will be resolved separately
        //     .ForMember(dest => dest.Cart, 
        //         opt => opt.Ignore()); // Cart will be resolved separately
    }
}