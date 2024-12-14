using AutoMapper;
using BookStoreTask.BookMod.Books.Repository;
using BookStoreTask.BookMod.Catograzation.Repoistories;
using BookStoreTask.Cart;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.Data;
using BookStoreTask.FilesMod;
using BookStoreTask.Orders;
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
    IBooksRepository BooksRepository { get; }
    IBaseCategoryRepository BaseCategoryRepository { get; }
    IGernesRepository GernesRepository { get; }
    IAuthorsRepository AuthorsRepository { get; }

    ICartRepo CartRepo { get; }

    IOrdersRepoisotry OrdersRepoisotry { get; }
    
    ICartItemRepoisotry CartItemRepoisotry { get; }
}

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly ProjectContext _repoContext;
    private readonly IMapper _mapper;
    private IFileRepository _fileRepository;
    private IUserRepository _userRepository;
    private IAdminRepository _adminRepository;
    private ICustomerRepository _customerRepository;
    private IBooksRepository _booksRepository;
    private IBaseCategoryRepository _baseCategoryRepository;
    private IGernesRepository _gernesRepository;
    private IAuthorsRepository _authorsRepository;
    private ICartRepo _cartRepo;
    private IOrdersRepoisotry _ordersRepoisotry;
    private ICartItemRepoisotry _cartItemRepoisotry;


    public RepositoryWrapper(ProjectContext repoContext, IMapper mapper)
    {
        _repoContext = repoContext;
        _mapper = mapper;
    }

    public IFileRepository FileRepository => _fileRepository ??= new FileRepository(_repoContext, _mapper);

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_repoContext, _mapper);

    public IAdminRepository AdminRepository => _adminRepository ??= new AdminRepository(_repoContext, _mapper);

    public ICustomerRepository CustomerRepository =>
        _customerRepository ??= new CustomerRepository(_repoContext, _mapper);

    public IBooksRepository BooksRepository =>
        _booksRepository ??= new BooksRepository(_repoContext, _mapper);

    public IBaseCategoryRepository BaseCategoryRepository =>
        _baseCategoryRepository ??= new BaseCategoryRepository(_repoContext, _mapper);

    public IGernesRepository GernesRepository =>
        _gernesRepository ??= new GernesRepository(_repoContext, _mapper);

    public IAuthorsRepository AuthorsRepository =>
        _authorsRepository ??= new AuthorsRepository(_repoContext, _mapper);

    public ICartRepo CartRepo =>
        _cartRepo ??= new CartRepo(_repoContext, _mapper);

    public IOrdersRepoisotry OrdersRepoisotry =>
        _ordersRepoisotry ??= new OrdersRepoisotry(_repoContext, _mapper);
    
    public ICartItemRepoisotry CartItemRepoisotry =>
        _cartItemRepoisotry ??= new CartItemRepoisotry(_repoContext, _mapper);
}