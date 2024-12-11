using AutoMapper;
using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser;
using BookStoreTask.Users.BaseUser.Dto;
using BookStoreTask.Users.Customers;
using BookStoreTask.Users.Payloads;

namespace BookStoreTask.Users.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        // General User Mapping
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage.FilePath))
            .ReverseMap();

        CreateMap<UserForm, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Handle password hashing separately
            .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

        CreateMap<UserUpdateForm, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Handle password updates separately
            .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

        // Admin Specific Mapping
        CreateMap<Admin, AdminDto>()
            .IncludeBase<User, UserDto>()
            .ReverseMap();

        CreateMap<AdminForm, Admin>()
            .IncludeBase<UserForm, User>()
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole));

        CreateMap<AdminUpdateForm, Admin>()
            .IncludeBase<UserUpdateForm, User>()
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole));

        // Customer Specific Mapping
        CreateMap<Customer, CustomerDto>()
            .IncludeBase<User, UserDto>()
            .ReverseMap();

        CreateMap<CustomerForm, Customer>()
            .IncludeBase<UserForm, User>();

        CreateMap<CustomerUpdateForm, Customer>()
            .IncludeBase<UserUpdateForm, User>()
            .ForMember(dest => dest.CustomerStatus, opt => opt.MapFrom(src => src.CustomerStatus));
    }
}