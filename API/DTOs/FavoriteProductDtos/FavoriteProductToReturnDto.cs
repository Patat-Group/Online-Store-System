using API.DTOs.ProductDtos;
using Core.Entities;

namespace API.DTOs.FavoriteProductDtos
{
    public class FavoriteProductToReturnDto
    {
        public string UserId { get; set; }
        public int  ProductId { get; set; }
        public ProductToReturnDto  Product { get; set; }
    }
}