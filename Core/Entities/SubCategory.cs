
using System.Collections.Generic;

namespace Core.Entities
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductAndSubCategory>? productAndSubCategories { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}