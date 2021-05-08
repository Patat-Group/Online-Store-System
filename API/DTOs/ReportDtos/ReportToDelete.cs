using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ReportDtos
{
    public class ReportToDelete
    {
        [Required]
        public int Id { get; set; }
    }
}