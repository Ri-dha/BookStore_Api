using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;

namespace BookStoreTask.Cart;

public interface ICartRepo : IBaseRepository<Carts, Guid>
{
}

public class CartRepo : BaseRepository<Carts, Guid>, ICartRepo
{
    private readonly ProjectContext _context;
    public CartRepo(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
}