using API.DTOs.ProductImagesDtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class ProductImagesUrlResolver : IValueResolver<ProductImage, ProductImagesForReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductImagesUrlResolver(IConfiguration config)
        {
            _config = config;

        }
        public string Resolve(ProductImage source, ProductImagesForReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return _config["ApiUrl"] +source.ImageUrl;
            }
            return null;
        }
    }
}