using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.BookMod.Catograzation.Author.utli;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Utli;
using BookStoreTask.FilesMod;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Catograzation.Dto;

public class BaseCategoryDto:BaseDto<Guid>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<BookDto>? Books { get; set; } 
    public CategoriesTypes? Type { get; set; }
}


public class AuthorDto:BaseCategoryDto
{
    public CountryEnum? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Biography { get; set; }
    public FilesDto? Image { get; set; }
}

public class GenereDto:BaseCategoryDto
{
    
}

