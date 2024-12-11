using System.Text;
using BookStoreTask.Auth;
using BookStoreTask.FilesMod;
using BookStoreTask.Users.Admins;
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
        
        
        services.AddAutoMapper(typeof(UserMapper));
        
        
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
        
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("AdminPolicy", policy =>
        //         policy.RequireClaim("role", Roles.Admin.ToString()));
        //
        //     options.AddPolicy("DriverPolicy", policy =>
        //         policy.RequireClaim("role", Roles.Driver.ToString()));
        //
        //     options.AddPolicy("CustomerPolicy", policy =>
        //         policy.RequireClaim("role", Roles.Customer.ToString()));
        //
        //     options.AddPolicy("ManagerPolicy", policy =>
        //         policy.RequireClaim("adminRole", AdministrativeRoles.Manager.ToString()));
        //
        //
        //     options.AddPolicy("AdminOrManagerPolicy", policy =>
        //         policy.RequireAssertion(context =>
        //             context.User.HasClaim(c => c.Type == "role" && c.Value == Roles.Admin.ToString()) &&
        //             context.User.HasClaim(c => c.Type == "adminRole" &&
        //                                        (c.Value == AdministrativeRoles.Manager.ToString() ||
        //                                         c.Value == AdministrativeRoles.Administrator.ToString()))));
        // });
        
        return services;
    }
}