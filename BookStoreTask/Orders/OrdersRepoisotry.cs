using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;

namespace BookStoreTask.Orders;

public interface IOrdersRepoisotry:IBaseRepository<OrdersModel,Guid>
{
    
}

public class OrdersRepoisotry:BaseRepository<OrdersModel,Guid>,IOrdersRepoisotry
{
    
    private readonly ProjectContext _context;

    public OrdersRepoisotry(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
    
}