using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Users.BaseUser.Repository;

public interface IUserRepository : IBaseRepository<User, Guid> {
    
    Task<List<Guid>> GetUsersIdsByRole(Roles role);
}

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    private readonly ProjectContext _db;

    public UserRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }

    public async Task<List<Guid>> GetUsersIdsByRole(Roles role)
    {
        return await _db.Users.Where(x => x.Role == role).Select(x => x.Id).ToListAsync();
    }
}