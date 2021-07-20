using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.ProductAndSubCategoryDtos;
using API.DTOs.SubCategoryDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/LinkProductWithSubCategory")]
    [ApiController]
    public class LinkProductWithSubCategoryController : ControllerBase
    {
        private readonly ISubCategoriesAndProduct _subCategoriesAndProduct;
        private readonly IGenericRepository<Product, int> _productRepository;
        private readonly IGenericRepository<SubCategory, int> _subCategoryRepository;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public LinkProductWithSubCategoryController(ISubCategoriesAndProduct subCategoriesAndProduct,
            IGenericRepository<Product, int> productRepository, IGenericRepository<SubCategory, int> subCategoryRepository,
            IUserRepository userRepo, IMapper mapper)
        {
            _subCategoriesAndProduct = subCategoriesAndProduct;
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet("{productId}")]
        public async Task<IReadOnlyList<SubCategoryToReturnDto>> GetSubCategoryByProduct(int productId)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null) return null;

            var subCategories = await _subCategoriesAndProduct.GetSubCategoryByProduct(productId);
            var data = _mapper.Map<IReadOnlyList<SubCategory>, IReadOnlyList<SubCategoryToReturnDto>>(subCategories);
            return data.ToList();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddSubCategoryToProduct([FromBody] ProductAndSubCategoryForAddDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var subCategory = await _subCategoryRepository.GetById(entity.SubCategoryId);
            if (subCategory == null)
                return BadRequest("Please check SubCategory exist.");

            var product = await _productRepository.GetById(entity.ProductId);
            if (product == null)
                return BadRequest("Please check product exist.");

            var dataForAdd = _mapper.Map<ProductAndSubCategory>(entity);

            if (await _subCategoriesAndProduct.AddSubCategoryToProduct(dataForAdd))
                return Ok();
            throw new Exception("Error happen when Add SubCategory To Product");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateSubCategoryToProduct([FromQuery] int id, ProductAndSubCategoryForAddDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var subCategory = await _subCategoryRepository.GetById(entity.SubCategoryId);
            if (subCategory == null)
                return BadRequest("Please check SubCategory exist.");

            var product = await _productRepository.GetById(entity.ProductId);
            if (product == null)
                return BadRequest("Please check product exist.");

            var dataForUpdate = await _subCategoriesAndProduct.GetById(id);
            dataForUpdate.ProductId = entity.ProductId;
            dataForUpdate.SubCategoryId = entity.SubCategoryId;
            
            if (await _subCategoriesAndProduct.UpdateSubCategoryWithProduct(dataForUpdate))
                return Ok();
            throw new Exception("Error happen when Update SubCategory To Product");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSubCategoryForProduct([FromQuery] int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var dateForDelete = await _subCategoriesAndProduct.DeleteSubCategoryWithProduct(id);
            if (dateForDelete)
                return Ok();
            throw new Exception("Error happen when Delete SubCategory To Product");
        }
    }
}