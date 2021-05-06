using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.DTOs.ProductImagesDtos
{
    public class ProductImageForAddDto
    {
        public ProductImageForAddDto()
        {
            DateAdded = DateTime.UtcNow;
        }

        [Required] 
        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public bool IsMainPhoto { get; set; } = false;
        public DateTime DateAdded { get; set; }
    }
}