using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<ProductImage> Images { get; set; }
    }
}