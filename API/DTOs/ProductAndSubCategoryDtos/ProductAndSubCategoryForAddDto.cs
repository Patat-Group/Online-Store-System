using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ProductAndSubCategoryDtos
{
    public class ProductAndSubCategoryForAddDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
    }
}