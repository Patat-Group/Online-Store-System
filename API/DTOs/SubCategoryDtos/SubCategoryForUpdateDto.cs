using System.ComponentModel.DataAnnotations;

namespace API.DTOs.SubCategoryDtos
{
    public class SubCategoryForUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}