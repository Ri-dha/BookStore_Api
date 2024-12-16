using AutoMapper;
using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.BookMod.Books.Payloads;

namespace BookStoreTask.BookMod.Books.mapper;

public class BooksMapper : Profile
{
    public BooksMapper()
    {
        CreateMap<BooksModel, BookDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        CreateMap<BookDto, BooksModel>();

        CreateMap<BookCreateForm, BooksModel>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Pages, opt => opt.MapFrom(src => src.Pages))
            .ForMember(dest => dest.PublishYear, opt => opt.MapFrom(src => src.PublishYear))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // Images will be handled separately
            
        CreateMap<BookUpdateForm, BooksModel>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        
        
    }
}