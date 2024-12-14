using System.Text;
using BookStoreTask.Auth;
using BookStoreTask.BookMod.Books.mapper;
using BookStoreTask.BookMod.Books.Repository;
using BookStoreTask.BookMod.Books.services;
using BookStoreTask.BookMod.Catograzation.Repoistories;
using BookStoreTask.BookMod.Catograzation.Services;
using BookStoreTask.Cart;
using BookStoreTask.FilesMod;
using BookStoreTask.Orders;
using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser;
using BookStoreTask.Users.BaseUser.Repository;
using BookStoreTask.Users.Customers;
using BookStoreTask.Users.Mapper;
using BookStoreTask.Users.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreTask.Utli;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<ICartRepo, CartRepo>();
        services.AddScoped<ICartServices, CartServices>();
        services.AddScoped<IBooksRepository,BooksRepository>();
        services.AddScoped<IBooksServices,BooksServices>();
        services.AddScoped<IAuthorsRepository,AuthorsRepository>();
        services.AddScoped<IGernesRepository,GernesRepository>();
        services.AddScoped<IBaseCategoryRepository,BaseCategoryRepository>();
        services.AddScoped<IAuthorServices,AuthorServices>();
        services.AddScoped<IGenreServices,GenreServices>();
        services.AddScoped<IOrdersRepoisotry,OrdersRepoisotry>();
        services.AddScoped<IOrdersService, OrdersService>();
        


        services.AddAutoMapper(
            typeof(UserMapper),
            typeof(ProjectFilesMapper),
            typeof(CartMapper),
            typeof(BooksMapper),
            typeof(OrdersMapper)
        );


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim("role", Roles.Admin.ToString()));

            options.AddPolicy("CustomerPolicy", policy =>
                policy.RequireClaim("role", Roles.Customer.ToString()));

            options.AddPolicy("ManagerPolicy", policy =>
                policy.RequireClaim("adminRole", AdministrativeRoles.Manager.ToString()));
        });

        return services;
    }
}