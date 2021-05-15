using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ReportDtos
{
    public class ReportToDeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
}