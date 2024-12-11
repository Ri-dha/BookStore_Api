using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.Utli;

namespace BookStoreTask.FilesMod;


public interface IFileRepository : IBaseRepository<ProjectFiles, Guid>
{
}
public class FileRepository: BaseRepository<ProjectFiles, Guid>, IFileRepository
{
    
    private readonly ProjectContext _context;
    
    public FileRepository(ProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
    
}