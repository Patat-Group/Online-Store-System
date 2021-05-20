using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ProductDtos
{
    public class ProductForCreationDto
    {
        public ProductForCreationDto()
        {
            DateAdded = DateTime.Now;
        }

        [Required, StringLength(60, ErrorMessage = "Product name must be less or equal 60 character.")]
        public string Name { get; set; }
        [StringLength(400, ErrorMessage = "ShortDescription must be less or equal 400 character.")]
        public string ShortDescription { get; set; }
        [StringLength(Int32.MaxValue)]
        public string LongDescription { get; set; }
        [Required, StringLength(60, ErrorMessage = "Category name must be less or equal 60 character.")]
        public string CategoryName { get; set; }
        public double Price { get; set; } = 0.0;
        public DateTime DateAdded { get; set; }
    }
}