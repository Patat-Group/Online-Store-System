using API.DTOs.Category;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Core.Entities;

namespace API.Helpers
{
    public class CategoryImageUrlResolver : IValueResolver<Category, CategoryReturnDto, string>
    {
        private readonly IConfiguration _config;
        public CategoryImageUrlResolver(IConfiguration config)
        {
            _config = config;

        }
        public string Resolve(Category source, CategoryReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return _config["ApiUrl"] + source.ImageUrl;
            }
            return null;
        }
    }
}