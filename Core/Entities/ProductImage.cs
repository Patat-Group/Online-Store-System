using System;

namespace Core.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMainPhoto { get; set; }
        public DateTime DateAdded { get; set; }
    }
}