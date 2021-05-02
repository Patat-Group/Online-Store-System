using API.DTOs.CategoryDtos;
using API.DTOs.SubCategoryDtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category,CategoryToReturnDto>();
            CreateMap<SubCategory,SubCategoryToReturnDto>();
        }
    }
}