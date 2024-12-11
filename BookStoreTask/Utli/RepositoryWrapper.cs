using AutoMapper;
using BookStoreTask.Data;
using BookStoreTask.FilesMod;
using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser.Repository;
using BookStoreTask.Users.Customers;

namespace BookStoreTask.Utli;

public interface IRepositoryWrapper
{
    IFileRepository FileRepository { get; }
    IUserRepository UserRepository { get; }
    IAdminRepository AdminRepository { get; }
    ICustomerRepository CustomerRepository { get; }
}

public class RepositoryWrapper: IRepositoryWrapper
{
    
    private readonly ProjectContext _repoContext;
    private readonly IMapper _mapper;
    private IFileRepository _fileRepository;
    private IUserRepository _userRepository;
    private IAdminRepository _adminRepository;
    private ICustomerRepository _customerRepository;
    
    public RepositoryWrapper(ProjectContext repoContext, IMapper mapper)
    {
        _repoContext = repoContext;
        _mapper = mapper;
    }
    
    public IFileRepository FileRepository => _fileRepository ??= new FileRepository(_repoContext, _mapper);

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_repoContext, _mapper);
    
    public IAdminRepository AdminRepository => _adminRepository ??= new AdminRepository(_repoContext, _mapper);

    public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_repoContext, _mapper);
    

}