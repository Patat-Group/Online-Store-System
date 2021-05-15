using System.Linq;
using API.DTOs.CategoryDtos;
using API.DTOs.FavoriteProductDtos;
using API.DTOs.ProductDtos;
using API.DTOs.ProductImagesDtos;
using API.DTOs.RatingDtos;
using API.DTOs.ReportDtos;
using API.DTOs.SubCategoryDtos;
using API.DTOs.UserDtos;
using API.DTOs.VIPAdsDtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        string apiUrl = @"http://localhost:5000/";
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>();
            CreateMap<SubCategory, SubCategoryToReturnDto>();
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.ImagesUrl,
                    opt => opt.MapFrom(
                        src => src.Images.Select(x => apiUrl + x.ImageUrl).ToList()));
            CreateMap<ProductImage, ProductImagesForReturnDto>()
                .ForMember(dest => dest.ImageUrl, src => src.MapFrom<ProductImagesUrlResolver>());
            CreateMap<ProductImage, ProductImageForAddDto>().ReverseMap();
            CreateMap<User, UserToReturnDto>();
            CreateMap<User, UserUpdateInformationDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserRated, UserDetailedRateDto>();
            CreateMap<Report, ReportToReturnDto>();
            CreateMap<Rating, RatingToReturnDto>();
            CreateMap<FavoriteProduct, FavoriteProductToReturnDto>();
            CreateMap<VIPAd, VIPAdsForRetutrnAdsDto>()
                .ForMember(dest => dest.ImageUrl, src => src.MapFrom<VIPAdsImagesUrlResolver>());
        }
    }
}