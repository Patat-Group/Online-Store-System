using System.Collections.Generic;

namespace API.DTOs.ProductDtos
{
    public class ProductsToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public IReadOnlyList<string> ImageUrl { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
    }
}