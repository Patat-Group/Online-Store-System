#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.ProductDtos;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepo;


        public ProductController(IGenericRepository<Product, int> productRepo,
            IGenericRepository<Category, int> categoryRepo,
            IMapper mapper,
             IUserRepository userRepo
        )
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        // Need Refactor..
        [HttpGet]
        public async Task<IReadOnlyList<ProductToReturnDto>> GetProducts([FromQuery] ProductParams? productParams)
        {
            var products = await _productRepo.GetAllWithPaging(productParams);
            var productForReturn = _mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return productForReturn;

            throw new Exception("Error happen when get products, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {

            var product = await _productRepo.GetById(id);

            var productForReturn = _mapper.Map<ProductToReturnDto>(product);
            return Ok(productForReturn);

            throw new Exception("Error happen when get product, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        // Need Edit
        [HttpPost("{userId}/addProduct/{categoryId}")]
        [Authorize]
        public async Task<IActionResult> AddProduct(string userId, int categoryId,
            [FromBody] ProductForCreationDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

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


            throw new Exception("Error happen when Add product, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        // Need Refactor..
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductForUpdateDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

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
            if (Math.Abs(entity.Price) > 0.0)
                product.Price = entity.Price;
            if (entity.LongDescription != null)
                product.LongDescription = entity.LongDescription;
            if (entity.ShortDescription != null)
                product.ShortDescription = entity.ShortDescription;

            if (await _productRepo.Update(product) == true)
                return Ok();

            throw new Exception("Error happen when update product, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _productRepo.Delete(id) == true)
                return Ok("Done");

            throw new Exception("Error happen when delete product, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }
    }
}