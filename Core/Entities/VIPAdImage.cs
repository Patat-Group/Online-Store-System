using System;

namespace Core.Entities
{
    public class VIPAdImage
    {
        public int Id { get; set; }
        public int VIPAdId { get; set; }
        public VIPAd VipAd { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
    }
}