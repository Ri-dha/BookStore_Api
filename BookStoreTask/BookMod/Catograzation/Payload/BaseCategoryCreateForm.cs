using BookStoreTask.BookMod.Catograzation.Author.utli;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Catograzation.Payload;

public class BaseCategoryCreateForm
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class AuthorCreateForm:BaseCategoryCreateForm
{
    public CountryEnum? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Biography { get; set; }
    public IFormFile? Image { get; set; }
}

public class GenereCreateForm:BaseCategoryCreateForm
{
    
}


public class BaseCategoryUpdateForm
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class AuthorUpdateForm:BaseCategoryUpdateForm
{
    public CountryEnum? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Biography { get; set; }
    public IFormFile? Image { get; set; }
}

public class GenereUpdateForm:BaseCategoryUpdateForm
{
    
}

public class BaseCategoryFilterForm:BaseFilter
{
    public string? Name { get; set; }
}

public class AuthorFilterForm:BaseCategoryFilterForm
{
    public CountryEnum? Country { get; set; }
}

public class GenereFilterForm:BaseCategoryFilterForm
{
    
}