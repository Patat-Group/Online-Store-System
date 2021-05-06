using API.Helpers;
using AutoMapper;
using Core.Entities;
using Interfaces.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Data;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x =>
                x.UseSqlite(_configuration.GetConnectionString("MyConnection")));
            
            services.AddScoped<IGenericRepository<Product,int>, ProductRepository>();
            services.AddScoped<IGenericRepository<VIPAd,int>, AdsRepository>();
            services.AddScoped<IGenericRepository<Category,int>, CategoryRepository>();
            services.AddScoped<IGenericRepository<SubCategory,int>, SubCategoryRepository>();
            services.AddScoped<IImageRepository, ProductImageRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseStaticFiles();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
    }
}
