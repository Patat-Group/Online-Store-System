using System.Collections.Generic;

namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}