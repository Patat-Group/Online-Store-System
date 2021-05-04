using System.Collections.Generic;

namespace API.DTOs.ProductDtos
{
    public class ProductForUpdateDto
    {
        public string Name { get; set; } = null;
        public string ShortDescription { get; set; } = null;
        public string LongDescription { get; set; } = null;
        public double Price { get; set; } = 0.0;
        public bool IsSold { get; set; } = false;
        public int CategoryId { get; set; } = 0;
    }
}