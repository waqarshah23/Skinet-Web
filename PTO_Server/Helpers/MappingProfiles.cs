using AutoMapper;
using Core.Models;
using PTO_Server.Dto;

namespace PTO_Server.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Products, ProductsToReturnDto>()
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.name))
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}
