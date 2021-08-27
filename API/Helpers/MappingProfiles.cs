using System.Linq;
using API.DTOs.Category;
using API.DTOs.FavoriteProductDtos;
using API.DTOs.ProductAndSubCategoryDtos;
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
            CreateMap<SubCategory, SubCategoryToReturnDto>();
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ImageUrl, src => src.MapFrom<CategoryImageUrlResolver>());
            CreateMap<SubCategoryForAddDto, SubCategory>();

            CreateMap<Product, ProductsToReturnDto>()
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(
                        src => src.Images.Where(im => im.IsMainPhoto).Select(x => apiUrl + x.ImageUrl)))
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src.User.Address))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.ImagesUrl,
                    opt => opt.MapFrom(
                        src => src.Images.Select(x => apiUrl + x.ImageUrl).ToList()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FacebookUrl, opt => opt.MapFrom(src => src.User.FacebookUrl))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.TelegramUrl, opt => opt.MapFrom(src => src.User.TelegramUrl))
                .ForMember(dest => dest.WhatsappUrl, opt => opt.MapFrom(src => src.User.WhatsappUrl));

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
            CreateMap<ProductAndSubCategoryForAddDto, ProductAndSubCategory>();
        }
    }
}