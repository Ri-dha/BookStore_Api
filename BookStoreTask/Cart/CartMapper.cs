using AutoMapper;
using BookStoreTask.Cart.CartItems;

namespace BookStoreTask.Cart;

public class CartMapper : Profile
{
    public CartMapper()
    {
        CreateMap<Carts, CartsDto>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

        CreateMap<CartsDto, Carts>();

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author.Name))
            .ForMember(dest => dest.BookImage, opt => opt.MapFrom(src => src.Book.Images))
            .ForMember(dest => dest.BookPrice, opt => opt.MapFrom(src => src.Book.Price));
    }
}