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
        public IFormFile File { get; set; }

        // [Required]
        // public bool IsMainPhoto { get; set; }

        public DateTime DateAdded { get; }
    }
}