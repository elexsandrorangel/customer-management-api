using AutoMapper;
using CustomerManagement.Models.Entities;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Infra.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Phones, PhoneViewModel>().ReverseMap();            
        }
    }
}
