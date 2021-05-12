using API.DTOs.VIPAdsDtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class VIPAdsImagesUrlResolver : IValueResolver<VIPAd, VIPAdsForRetutrnAdsDto, string>
    {
        private readonly IConfiguration _config;
        public VIPAdsImagesUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(VIPAd source, VIPAdsForRetutrnAdsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
                return _config["ApiUrl"] + source.ImageUrl;
            return null;
        }

    }
}