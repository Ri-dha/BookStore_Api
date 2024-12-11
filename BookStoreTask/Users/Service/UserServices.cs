using AutoMapper;
using BookStoreTask.Auth;
using BookStoreTask.FilesMod;
using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser;
using BookStoreTask.Users.BaseUser.Dto;
using BookStoreTask.Users.Customers;
using BookStoreTask.Users.Payloads;
using BookStoreTask.Utli;

namespace BookStoreTask.Users.Service;

public interface IUserServices
{
    Task<(UserDto? user, string? error)> Delete(Guid id);
    Task<(UserDto? user, string? error)> Get(Guid id);
    Task<(UserDto? user, string? error)> Login(LoginForm loginForm);
    Task<(List<CustomerDto>? user, int? totalCount, string? error)> GetAllCustomers(CustomerFilter filter);
    Task<(List<AdminDto>? user, int? totalCount, string? error)> GetAllAdmins(AdminFilter filter);
    Task<(List<object>? rolesList, string? error)> GetRoles();
    Task<(List<object>? adminRolesList, string? error)> GetAdminRoles();
    Task<(UserDto? userDto, string? error)> RegisterCustomer(CustomerForm customerForm,IFormFile? profileImage);
    Task<(UserDto? userDto, string? error)> UpdateCustomer(CustomerUpdateForm customerUpdateForm, Guid id);
    Task<(UserDto? userDto, string? error)> ChangePassword(string oldPassword, string newPassword, Guid id);
    Task<(UserDto? userDto, string? error)> RegisterAdmin(AdminForm adminForm,IFormFile? profileImage);
    Task<(UserDto? userDto, string? error)> UpdateAdmin(AdminUpdateForm updateForm, Guid id);
}

public class UserServices : IUserServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IFileService _fileService;

    public UserServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService,
        IFileService fileService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _tokenService = tokenService;
        _fileService = fileService;
    }

    public async Task<(UserDto? user, string? error)> Delete(Guid id)
    {
        var user = await _repositoryWrapper.UserRepository.Get(x => x.Id == id);
        if (user == null)
        {
            return (null, "User not found");
        }

        await _repositoryWrapper.UserRepository.SoftDelete(id);
        return (_mapper.Map<UserDto>(user), null);
    }

    public async Task<(UserDto? user, string? error)> Get(Guid id)
    {
        var user = await _repositoryWrapper.UserRepository.Get(x => x.Id == id);
        if (user == null)
        {
            return (null, "User not found");
        }

        if (user.Role == Roles.Admin)
        {
            var admin = await _repositoryWrapper.AdminRepository.Get(x => x.Id == id);
            if (admin == null)
            {
                return (null, "Admin not found");
            }

            return (_mapper.Map<UserDto>(user), null);
        }

        if (user.Role == Roles.Customer)
        {
            var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == id);
            if (customer == null)
            {
                return (null, "Customer not found");
            }

            return (_mapper.Map<UserDto>(user), null);
        }

        return (_mapper.Map<UserDto>(user), null);
    }

    public async Task<(UserDto? user, string? error)> Login(LoginForm loginForm)
    {
        var user = await _repositoryWrapper.UserRepository.Get(x => x.Email == loginForm.email);
        if (user == null)
        {
            return (null, "User not found");
        }

        if (user.Deleted)
        {
            return (null, "User is deleted");
        }

        if (user.Role == Roles.Customer)
        {
            var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == user.Id);
            if (customer == null)
            {
                return (null, "Customer not found");
            }

            if (customer.CustomerStatus == CustomerStatus.Suspended)
            {
                return (null, "Customer is blocked");
            }
        }

        if (!BCrypt.Net.BCrypt.Verify(loginForm.Password, user.Password))
        {
            return (null, "Password or email are incorrect");
        }

        user.LastLogin = DateTime.Now;
        await _repositoryWrapper.UserRepository.Update(user, user.Id);
        var token = _tokenService.CreateToken(user);
        var dto = _mapper.Map<UserDto>(user);
        dto.Token = token;
        return (dto, null);
    }


    public async Task<(List<CustomerDto>? user, int? totalCount, string? error)> GetAllCustomers(CustomerFilter filter)
    {
        var (customers, totalCount) = await _repositoryWrapper.CustomerRepository.GetAll<CustomerDto>(
            x =>
                (filter.CustomerStatus == null || x.CustomerStatus == filter.CustomerStatus) &&
                (string.IsNullOrEmpty(filter.PhoneNumber) || x.PhoneNumber.Contains(filter.PhoneNumber)) &&
                (string.IsNullOrEmpty(filter.Email) || x.Email.Contains(filter.Email)) &&
                (filter.Role == null || x.Role == filter.Role) &&
                (x.Deleted == false)
            ,
            filter.PageNumber, filter.PageSize
        );


        return (customers, totalCount, null);
    }

    public async Task<(List<AdminDto>? user, int? totalCount, string? error)> GetAllAdmins(AdminFilter filter)
    {
        var (admins, totalCount) = await _repositoryWrapper.AdminRepository.GetAll<AdminDto>(
            x =>
                (filter.AdministrativeRole == null || x.AdministrativeRole == filter.AdministrativeRole) &&
                (string.IsNullOrEmpty(filter.PhoneNumber) || x.PhoneNumber.Contains(filter.PhoneNumber)) &&
                (string.IsNullOrEmpty(filter.Email) || x.Email.Contains(filter.Email)) &&
                (filter.Role == null || x.Role == filter.Role) &&
                (x.Deleted == false)
            ,
            filter.PageNumber, filter.PageSize
        );
        return (admins, totalCount, null);
    }

    public async Task<(List<object>? rolesList, string? error)> GetRoles()
    {
        var roles = Enum.GetValues<Roles>()
            .Cast<Roles>()
            .Select(role => new { Name = role.ToString(), Value = (int)role })
            .ToList<object>();
        return (roles, null);
    }

    public async Task<(List<object>? adminRolesList, string? error)> GetAdminRoles()
    {
        var roles = Enum.GetValues<AdministrativeRoles>()
            .Cast<AdministrativeRoles>()
            .Select(role => new { Name = role.ToString(), Value = (int)role })
            .ToList<object>();
        return (roles, null);
    }

    public async Task<(UserDto? userDto, string? error)> RegisterCustomer(CustomerForm customerForm,IFormFile? profileImage)
    {
        var customer = await _repositoryWrapper.UserRepository.Get(x => x.Email == customerForm.Email);
        if (customer != null)
        {
            return (null, "Email already exists");
        }

        var user = new Customer()
        {
            Email = customerForm.Email,
            Username = customerForm.Username,
            PhoneNumber = customerForm.PhoneNumber,
        };
        user.Role = Roles.Customer;
        user.Password = BCrypt.Net.BCrypt.HashPassword(customerForm.Password);
        if (profileImage != null)
        {
            var image = await _fileService.SaveFileAsync(profileImage);
            user.ProfileImage = image;
        }
        user.Role = Roles.Customer;

        await _repositoryWrapper.CustomerRepository.Add(user);
        var dto = _mapper.Map<UserDto>(user);
        dto.Token = _tokenService.CreateToken(user);
        return (dto, null);
    }

    public async Task<(UserDto? userDto, string? error)> UpdateCustomer(CustomerUpdateForm customerUpdateForm, Guid id)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == id);
        if (customer == null)
        {
            return (null, "Customer not found");
        }

        _mapper.Map(customerUpdateForm, customer);
        if (customerUpdateForm.ProfileImage != null)
        {
            var image = await _fileService.SaveFileAsync(customerUpdateForm.ProfileImage);
            customer.ProfileImage = image;
        }

        await _repositoryWrapper.CustomerRepository.Update(customer, id);
        var customerDto = _mapper.Map<CustomerDto>(customer);
        return (customerDto, null);
    }

    public async Task<(UserDto? userDto, string? error)> ChangePassword(string oldPassword, string newPassword, Guid id)
    {
        var user = await _repositoryWrapper.UserRepository.Get(x => x.Id == id);
        if (user == null)
        {
            return (null, "User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
        {
            return (null, "Old password is incorrect");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _repositoryWrapper.UserRepository.Update(user, id);
        var dto = _mapper.Map<UserDto>(user);
        return (dto, null);
    }

    public async Task<(UserDto? userDto, string? error)> RegisterAdmin(AdminForm adminForm,IFormFile? profileImage)
    {
        var admin = await _repositoryWrapper.AdminRepository.Get(x => x.Email == adminForm.Email);
        if (admin != null)
        {
            return (null, "Email already exists");
        }

        var user = _mapper.Map<Admin>(adminForm);
        user.Role = Roles.Admin;
        user.Password = BCrypt.Net.BCrypt.HashPassword(adminForm.Password);
        if (profileImage != null)
        {
            var image = await _fileService.SaveFileAsync(profileImage);
            user.ProfileImage = image;
        }
        user.AdministrativeRole = adminForm.AdministrativeRole;

        await _repositoryWrapper.AdminRepository.Add(user);
        var dto = _mapper.Map<UserDto>(user);
        dto.Token = _tokenService.CreateToken(user);
        return (dto, null);
    }

    public async Task<(UserDto? userDto, string? error)> UpdateAdmin(AdminUpdateForm updateForm, Guid id)
    {
        var admin = await _repositoryWrapper.AdminRepository.Get(x => x.Id == id);
        if (admin == null)
        {
            return (null, "Admin not found");
        }

        _mapper.Map(updateForm, admin);
        if (updateForm.ProfileImage != null)
        {
            var image = await _fileService.SaveFileAsync(updateForm.ProfileImage);
            admin.ProfileImage = image;
        }

        await _repositoryWrapper.AdminRepository.Update(admin, id);
        var adminDto = _mapper.Map<AdminDto>(admin);
        return (adminDto, null);
    }
}