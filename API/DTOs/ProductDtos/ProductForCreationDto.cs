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

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string CategoryName { get; set; }
        public double Price { get; set; } = 0.0;
        public DateTime DateAdded { get; set; }
    }
}