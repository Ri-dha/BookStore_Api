using BookStoreTask.BookMod.Catograzation.Author.utli;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Model;
using BookStoreTask.FilesMod;

namespace BookStoreTask.BookMod.Catograzation.Author.model;

public class Authors:BaseCategory
{
    public CountryEnum? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Biography { get; set; }
    public ProjectFiles? Image { get; set; }
}