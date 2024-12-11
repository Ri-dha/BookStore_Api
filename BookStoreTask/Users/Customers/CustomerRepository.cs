using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Users.Customers;

public interface ICustomerRepository : IBaseRepository<Customer, Guid>
{
    Task<List<Customer>> GetAllCustomers();
}

public class CustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    private readonly ProjectContext _context;

    public CustomerRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        return await _context.Customers.ToListAsync();
    }
}