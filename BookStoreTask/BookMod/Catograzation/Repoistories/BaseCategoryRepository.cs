using AutoMapper;
using BookStoreTask.BookMod.Catograzation.Author.model;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Model;
using BookStoreTask.BookMod.Catograzation.Genere.Model;
using BookStoreTask.Data;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Catograzation.Repoistories;

public interface IBaseCategoryRepository:IBaseRepository<BaseCategory,Guid>
{
    
}

public class BaseCategoryRepository:BaseRepository<BaseCategory,Guid>,IBaseCategoryRepository
{
    
    private readonly ProjectContext _context;
    
    public BaseCategoryRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
}

public interface IGernesRepository:IBaseRepository<Genres,Guid>
{
    
}

public class GernesRepository:BaseRepository<Genres,Guid>,IGernesRepository
{
    
    private readonly ProjectContext _context;
    
    public GernesRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
}


public interface IAuthorsRepository:IBaseRepository<Authors,Guid>
{
    
}

public class AuthorsRepository:BaseRepository<Authors,Guid>,IAuthorsRepository
{
    
    private readonly ProjectContext _context;
    
    public AuthorsRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
}