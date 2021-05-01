using System.Collections.Generic;

namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}