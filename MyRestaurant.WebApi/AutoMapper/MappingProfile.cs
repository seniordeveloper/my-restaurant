using AutoMapper;
using MyRestaurant.ApiModels;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.WebApi.AutoMapper
{
    /// <summary>
    /// Organizes mapping configurations with <see cref="Profile" />.
    /// </summary>
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<TableEntity, TableModel>()
               .ReverseMap()
               .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CustomerEntity, CustomerModel>()
               .ReverseMap()
               .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ClientsGroupEntity, ClientsGroupModel>()
               .ReverseMap()
               .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
