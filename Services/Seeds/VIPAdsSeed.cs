using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Services.Data;

namespace Services.Seeds
{
    public class VIPAdsSeed
    {
        public static async Task SeedVIPAds(StoreContext context)
        {
            if (!context.VIPAds.Any())
            {
                var vipAdsData = await File.ReadAllTextAsync("../Services/Seeds/Data/VIPAds.json");
                var vipAds = JsonSerializer.Deserialize<List<VIPAd>>(vipAdsData);
                foreach (var vipAd in vipAds)
                {
                    var vipAdForAdd = new VIPAd
                    {
                        Name = vipAd.Name.ToLower(),
                        ImageUrl = vipAd.ImageUrl,
                        DateAdded = DateTime.UtcNow
                    };

                    await context.AddAsync(vipAdForAdd);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}