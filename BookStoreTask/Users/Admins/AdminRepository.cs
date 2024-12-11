using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Users.Admins;

public interface IAdminRepository:IBaseRepository<Admin,Guid>
{
    Task<List<Admin>> getAllAdmins();
}


public class AdminRepository:BaseRepository<Admin,Guid>,IAdminRepository
{
    private readonly ProjectContext _context;

    public AdminRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public async Task<List<Admin>> getAllAdmins()
    {
        return await _context.Admins.ToListAsync();
    }
}