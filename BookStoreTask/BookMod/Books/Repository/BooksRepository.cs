using AutoMapper;
using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.Data;
using BookStoreTask.Utli;

namespace BookStoreTask.BookMod.Books.Repository;

public interface IBooksRepository : IBaseRepository<BooksModel, Guid>
{
}

public class BooksRepository : BaseRepository<BooksModel, Guid>, IBooksRepository
{
    private readonly ProjectContext _context;

    public BooksRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
}