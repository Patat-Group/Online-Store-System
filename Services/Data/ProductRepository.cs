// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Core.Entities;
// using Interfaces.Core;
// using Microsoft.EntityFrameworkCore;
//
// namespace Services.Data
// {
//     public class ProductRepository :IGenericRepository<Product,int>
//     {
//         private readonly StoreContext _context;
//         
//         public ProductRepository(StoreContext context)
//         {
//             _context = context;
//         }
//
//         public async Task<IReadOnlyList<Product>> GetALl()
//         {
//             var products = await _context.Products
//                 .Include(im =>im.Images)
//                 .ToListAsync();
//             return products;
//         }
//         
//         public async Task<Product> GetById(int id)
//         {
//             var product =await _context.Products
//                 .Include(im =>im.Images)
//                 .FirstOrDefaultAsync(p =>p.Id ==id);
//             return product;
//         }
//
//         public async Task<bool> Delete(int id)
//         {
//             var product = await _context.Products
//                 .FirstOrDefaultAsync(p => p.Id == id);
//             if (product == null) return false;
//             _context.Products.Remove(product);
//             if(await SaveChanges())
//                 return true;
//             return false;
//         }
//
//         public async Task<bool> Add(Product entity)
//         {
//             if (entity == null) return false;
//             var productExist = await _context.Products
//                 .FirstOrDefaultAsync(p => p.Id == entity.Id);
//             if (productExist != null) return false;
//             
//             var product = new Product()
//             {
//                 Id = entity.Id,
//                 Name = entity.Name,
//                 Price = entity.Price,
//                 LongDescription = entity.LongDescription,
//                 ShortDescription = entity.ShortDescription,
//                 Images = entity.Images,
//                 DateAdded = entity.DateAdded
//             };
//             
//             await _context.Products.AddAsync(product);
//             if(await SaveChanges())
//                 return true;
//             return false;
//         }
//
//         public async Task<bool> Update(int id ,Product entity)
//         {
//             if (entity == null) return false;
//             var product = await _context.Products
//                 .FirstOrDefaultAsync(p => p.Id == id);
//             if (product == null) return false;
//
//             product.Name = entity.Name;
//             product.Price = entity.Price;
//             product.LongDescription = entity.LongDescription;
//             product.ShortDescription = entity.ShortDescription;
//             
//             if(await SaveChanges())
//                 return true;
//             return false;
//         }
//
//         // public async Task<bool> AddImage(int productId, ProductImage productImage)
//         // {
//         //     if (productImage == null) return false;
//         //     var product = await GetById(productId);
//         //     if (product == null) return false;
//         //     
//         //     var image =new ProductImage()
//         //     {
//         //         Id =productImage.Id,
//         //         DateAdded = DateTime.Now,
//         //         ProductId = product.Id,
//         //         ImageUrl = productImage.ImageUrl,
//         //         IsMainPhoto = productImage.IsMainPhoto
//         //     };
//         //
//         //     // var images = await GetAllImages(productId);
//         //     // bool flag = false;
//         //     // foreach (var item in images)
//         //     // {
//         //     //     if (item.IsMainPhoto == true && item.Id != productImage.Id)
//         //     //         flag = true;
//         //     // }
//         //     // if (flag) image.IsMainPhoto = false;
//         //     
//         //     if(await SaveChanges())
//         //         return true;
//         //     return false;
//         // }
//         //
//         // public async Task<bool> DeleteImage(int id)
//         // {
//         //     var image = await _context.ProductImages
//         //         .FirstOrDefaultAsync(im => im.Id ==id);
//         //     if (image == null) return false;
//         //
//         //     _context.Remove(image);
//         //     if(await SaveChanges())
//         //         return true;
//         //     return false;
//         // }
//         //
//         // public async Task<IReadOnlyList<ProductImage>> GetAllImages(int productId)
//         // {
//         //     var images = await _context.ProductImages
//         //         .Where(pd =>pd.ProductId ==productId)
//         //         .ToListAsync();
//         //     return images;
//         // }
//         //
//         // public async Task<ProductImage> GetMainPhotoForProduct(int productId)
//         // {
//         //     var image = await _context.ProductImages
//         //         .FirstOrDefaultAsync(im =>im.ProductId ==productId && im.IsMainPhoto);
//         //     return image;
//         // }
//         // public async Task<ProductImage> GetImageById(int id)
//         // {
//         //     var image = await _context.ProductImages
//         //         .FirstOrDefaultAsync(im =>im.Id ==id);
//         //     return image;
//         // }
//
//         public async Task<bool> SaveChanges()
//         {
//             return await _context.SaveChangesAsync() >0 ? true :false;
//         }
//     }
// }