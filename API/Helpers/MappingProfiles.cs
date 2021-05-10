using System.Linq;
using API.DTOs.CategoryDtos;
using API.DTOs.ProductDtos;
using API.DTOs.ProductImagesDtos;
using API.DTOs.ReportDtos;
using API.DTOs.SubCategoryDtos;
using API.DTOs.UserDtos;
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
            CreateMap<ProductImage, ProductImagesForReturnDto>();
            CreateMap<ProductImage, ProductImageForAddDto>().ReverseMap();
            CreateMap<User, UserToReturnDto>();
            CreateMap<User, UserUpdateInformationDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserRated, UserDetailedRateDto>();
            CreateMap<Report, ReportToReturnDto>();
        }
    }
}