using AutoMapper;
using MyAppCore.Entities;
using MyAppCore.Dtos;

namespace MyAppCore.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CustomerUpdateDto, Customer>();

            // Stock
            CreateMap<Stock, StockDto>().ReverseMap();
            CreateMap<StockUpdateDto, Stock>().ReverseMap();

            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
