using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.VIPAdsDtos
{
    public class VIPAdsForCreationDto
    {
        public VIPAdsForCreationDto()
        {
            DateAdded = DateTime.Now;
        }

        [Required, StringLength(120, ErrorMessage = "Name must be has less or equal 120 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This Field is require.")]
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
    }
}