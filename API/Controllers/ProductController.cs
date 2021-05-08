using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.ProductDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Need Edit..
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product, int> _productRepo;
        private readonly IGenericRepository<Category, int> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product, int> productRepo,
            IGenericRepository<Category, int> categoryRepo,
            IMapper mapper
        )
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        // Need Refactor..
        [HttpGet]
        public async Task<IReadOnlyList<ProductsToReturnDto>> GetProducts()
        {
            var products = await _productRepo.GetAll();
            var productForReturn = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products);
            
            return productForReturn;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepo.GetById(id);

            var productForReturn = _mapper.Map<ProductsToReturnDto>(product);
            return Ok(productForReturn);
        }

        // Need Edit
        [HttpPost("{userId}/addProduct/{categoryId}")]
        public async Task<IActionResult> AddProduct(string userId, int categoryId,
            [FromBody] ProductForCreationDto entity)
        {
            if (await _categoryRepo.GetById(categoryId) == null)
                return BadRequest("Category is not exist");

            var product = new Product()
            {
                Name = entity.Name,
                Price = entity.Price,
                LongDescription = entity.LongDescription,
                ShortDescription = entity.ShortDescription,
                DateAdded = entity.DateAdded,
                CategoryId = categoryId,
                UserId = userId
            };

            if (await _productRepo.Add(product) == true)
                return Ok();

            return BadRequest("Error happen when adding product");
        }

        // Need Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductForUpdateDto entity)
        {
            var product = await _productRepo.GetById(id);
            if (product == null)
                return BadRequest(@"you can't update product not exist");

            if (entity.CategoryId != 0)
            {
                var category = await _categoryRepo.GetById(entity.CategoryId);
                if (category == null)
                    return BadRequest(@"you can't update category not exist for product");
                product.CategoryId = entity.CategoryId;
            }

            if (entity.IsSold != false)
                product.IsSold = entity.IsSold;
            if (entity.Name != null)
                product.Name = entity.Name;
            if (entity.Price != 0.0)
                product.Price = entity.Price;
            if (entity.LongDescription != null)
                product.LongDescription = entity.LongDescription;
            if (entity.ShortDescription != null)
                product.ShortDescription = entity.ShortDescription;

            if (await _productRepo.Update(product) == true)
                return Ok();
            return BadRequest("Error happen when updating product");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (await _productRepo.Delete(id) == true)
                return Ok("Done");
            return BadRequest("Error happen when deleting product");
        }
    }
}