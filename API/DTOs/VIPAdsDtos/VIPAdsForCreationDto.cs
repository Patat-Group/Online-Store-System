using System;

namespace API.DTOs.VIPAdsDtos
{
    public class VIPAdsForCreationDto
    {
        public VIPAdsForCreationDto()
        {
            DateAdded =DateTime.Now;    
        }
        
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
    }
}