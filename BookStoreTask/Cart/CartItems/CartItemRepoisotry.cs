using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;

namespace BookStoreTask.Cart.CartItems;

public interface ICartItemRepoisotry:IBaseRepository<CartItem,Guid>
{
    
}


public class CartItemRepoisotry:BaseRepository<CartItem,Guid>,ICartItemRepoisotry
{
        
        private readonly ProjectContext _context;
    
        public CartItemRepoisotry(ProjectContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }
    
}