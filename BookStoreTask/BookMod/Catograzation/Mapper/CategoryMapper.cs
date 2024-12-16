using AutoMapper;
using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.BookMod.Catograzation.Author.model;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Model;
using BookStoreTask.BookMod.Catograzation.Dto;
using BookStoreTask.BookMod.Catograzation.Genere.Model;
using BookStoreTask.BookMod.Catograzation.Payload;
using BookStoreTask.FilesMod;

namespace BookStoreTask.BookMod.Catograzation.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        // Map BaseCategory to BaseCategoryDto
        CreateMap<BaseCategory, BaseCategoryDto>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                AuthorId = book.AuthorId,
                GenreId = book.GenreId,
                PublishYear = book.PublishYear,
                Pages = book.Pages
            }).ToList()));

        // Map Author to AuthorDto
        CreateMap<Authors, AuthorDto>()
            .IncludeBase<BaseCategory, BaseCategoryDto>() // Reuse BaseCategory mapping
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null
                ? new FilesDto
                {
                    Id = src.Image.Id,
                    FileName = src.Image.FileName,
                    FilePath = src.Image.FilePath,
                }
                : null));

        // Map Genres to GenereDto
        CreateMap<Genres, GenereDto>()
            .IncludeBase<BaseCategory, BaseCategoryDto>(); // Reuse BaseCategory mapping

        // Map AuthorCreateForm to Authors
        CreateMap<AuthorCreateForm, Authors>()
            .ForMember(dest => dest.Image, opt => opt.Ignore()); // File handling is separate

        // Map GenereCreateForm to Genres
        CreateMap<GenereCreateForm, Genres>();

        // Map AuthorUpdateForm to Authors
        CreateMap<AuthorUpdateForm, Authors>()
            .ForMember(dest => dest.Image, opt => opt.Ignore()) // File handling is separate
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Map GenereUpdateForm to Genres
        CreateMap<GenereUpdateForm, Genres>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values

        // Map AuthorFilterForm to AuthorDto (for filtering)
        CreateMap<AuthorFilterForm, AuthorDto>();

        // Map GenereFilterForm to GenereDto (for filtering)
        CreateMap<GenereFilterForm, GenereDto>();
    }
}