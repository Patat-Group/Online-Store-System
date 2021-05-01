// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Core.Entities;
// using Interfaces.Core;
// using Microsoft.AspNetCore.Mvc;
//
// namespace API.Controllers
// {
//     
//     // Need Edit..
//     [Route("api/Product")]
//     [ApiController]
//     public class ProductController :ControllerBase
//     {
//         private readonly IGenericRepository<Product, int> _productRepo;
//         public ProductController(IGenericRepository<Product,int> productRepo)
//         {
//             _productRepo = productRepo;
//         }
//         
//         [HttpGet]
//         public async Task<IReadOnlyList<Product>> GetProducts()
//         {
//             var products = await _productRepo.GetALl();
//             return products;
//         }
//         
//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetProduct(int id)
//         {
//             var product = await _productRepo.GetById(id);
//             return Ok(product);
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> AddProduct(Product entity)
//         {
//             if (!ModelState.IsValid) return BadRequest("Data input is not valid!");
//             if (await _productRepo.Add(entity) ==true)
//                 return NoContent();
//             return BadRequest("Error happen when adding product");
//         }
//         
//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateProduct(int id,Product entity)
//         {
//             if (!ModelState.IsValid) return BadRequest("Data input is not valid!");
//             if (await _productRepo.Update(id,entity) == true)
//                 return NoContent();
//             return BadRequest("Error happen when updating product");
//         }
//
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteProduct(int id)
//         {
//             if (await _productRepo.Delete(id) == true)
//                 return Ok("Done");
//             return BadRequest("Error happen when deleting product");
//         }
//     }
// }