#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.ProductDtos;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product, int> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;


        public ProductController(IGenericRepository<Product, int> productRepo, IMapper mapper, IUserRepository userRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IReadOnlyList<ProductsToReturnDto>> GetProducts([FromQuery] ProductParams? productParams)
        {
            var products = await _productRepo.GetAllWithSpec(productParams);
            var productForReturn = _mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products);

            Response.AddPagination(productParams.PageNumber, productParams.PageSize,
               productParams.MaxPageNumber, productParams.NumberOfPages);

            return productForReturn;

            throw new Exception("Error happen when get products..");
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IReadOnlyList<ProductsToReturnDto>> GetProductsByCategoryName(int categoryId, [FromQuery] ProductParams? productParams)
        {
            var products = await _productRepo.GetProductsByCategory(categoryId, productParams);
            var productForReturn = _mapper
                            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products);

            Response.AddPagination(productParams.PageNumber, productParams.PageSize,
               productParams.MaxPageNumber, productParams.NumberOfPages);
               
            return productForReturn;

            throw new Exception("Error happen when get products");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {

            var product = await _productRepo.GetById(id);

            var productForReturn = _mapper.Map<ProductToReturnDto>(product);
            return Ok(productForReturn);

            throw new Exception("Error happen when get product");
        }

        [HttpPost("addProduct")]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductForCreationDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var product = new Product()
            {
                Name = entity.Name.ToLower(),
                Price = entity.Price,
                LongDescription = entity.LongDescription,
                ShortDescription = entity.ShortDescription,
                DateAdded = entity.DateAdded,
                UserId = user.Id
            };

            if (await _productRepo.Add(product) == true)
                return Ok();

            throw new Exception("Error happen when Add product");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductForUpdateDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var product = await _productRepo.GetById(id);
            if (product == null)
                return BadRequest(@"you can't update product not exist");

            if (product.UserId != user.Id)
                return Unauthorized("You cannot Update a product owned by another user");

            if (entity.IsSold != false)
                product.IsSold = entity.IsSold;
            if (entity.Name != null)
                product.Name = entity.Name.ToLower();
            if (Math.Abs(entity.Price) > 0.0)
                product.Price = entity.Price;
            if (entity.LongDescription != null)
                product.LongDescription = entity.LongDescription;
            if (entity.ShortDescription != null)
                product.ShortDescription = entity.ShortDescription;

            if (await _productRepo.Update(product) == true)
                return Ok();

            throw new Exception("Error happen when update product");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var product = await _productRepo.GetById(id);
            if (product == null)
                return BadRequest(@"you can't delete product not exist");

            if (product.UserId != user.Id)
                return Unauthorized("You cannot Delete a product owned by another user");

            if (await _productRepo.Delete(id) == true)
                return Ok("Done");

            throw new Exception("Error happen when delete product");
        }
    }
}