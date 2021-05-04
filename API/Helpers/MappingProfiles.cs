using System.Linq;
using API.DTOs.CategoryDtos;
using API.DTOs.ProductDtos;
using API.DTOs.SubCategoryDtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>();
            CreateMap<SubCategory, SubCategoryToReturnDto>();
            CreateMap<Product, ProductsToReturnDto>()
                .ForMember(dest => dest.ImagesUrl,
                    opt => opt.MapFrom(
                        src => src.Images.ToList()));
        }
    }
}