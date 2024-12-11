using AutoMapper;
using BookStoreTask.Data;

namespace BookStoreTask.Utli;

public interface IRepositoryWrapper
{
    
}

public class RepositoryWrapper: IRepositoryWrapper
{
    
    private ProjectContext _repoContext;
    private IMapper _mapper;
    
    public RepositoryWrapper(ProjectContext repoContext, IMapper mapper)
    {
        _repoContext = repoContext;
        _mapper = mapper;
    }
    
}