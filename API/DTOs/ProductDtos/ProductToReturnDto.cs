using System;
using System.Collections.Generic;

namespace API.DTOs.ProductDtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string FacebookUrl { get; set; }
        public string phoneNumber { get; set; }
        public string WhatsappUrl { get; set; }
        public string TelegramUrl { get; set; }
        public IReadOnlyList<string> ImagesUrl { get; set; }
    }
}