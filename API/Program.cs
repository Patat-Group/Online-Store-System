using System;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Data;
using Services.Seeds;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var scope = host.Services.CreateScope();
            using (scope)
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    await context.Database.MigrateAsync();
                    await UserSeed.SeedUserAsync(userManager, context);
                    await CategorySeed.SeedCategory(context);
                    await SubCategorySeed.SeedSubCategory(context);
                    await VIPAdsSeed.SeedVIPAds(context);
                    await ProductSeed.SeedProductAsync(context, userManager);
                    await ProductImagesSeed.SeedProductImages(context);
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "error happen in migration");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
