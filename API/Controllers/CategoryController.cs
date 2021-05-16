using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CategoryDtos;
using API.DTOs.SubCategoryDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category, int> _categoryRepo;
        private readonly IGenericRepository<SubCategory, int> _subCategoryRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public CategoryController(IGenericRepository<Category, int> categoryRepo,
            IGenericRepository<SubCategory, int> subCategoryRepo, IUserRepository userRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<CategoryToReturnDto>> GetAll()
        {
            var categories = await _categoryRepo.GetAll();
            var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories);
            return data.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryToReturnDto>> GetById(int id)
        {
            var category = await _categoryRepo.GetById(id);
            var data = _mapper.Map<Category, CategoryToReturnDto>(category);
            if (category != null) return Ok(data);
            throw new Exception("Error happen when get Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(Category category)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _categoryRepo.GetById(category.Id) != null)
                return BadRequest("This category is exist.");

            var categoryForAdd = new Category()
            {
                Name = category.Name
            };

            if (await _categoryRepo.Add(categoryForAdd))
                return Ok();
            throw new Exception("Error happen when add Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, Category entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var category = await _categoryRepo.GetById(id);
            if (category == null)
                return BadRequest("This category is not exist!");
            category.Name = entity.Name;
            if (await _categoryRepo.Update(category))
                return Ok();
            throw new Exception("Error happen when update Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var category = await _categoryRepo.GetById(id);
            if (category == null)
                return BadRequest("Please check Category exist!");

            if (await _categoryRepo.Delete(id))
                return Ok();
            throw new Exception("Error happen when delete Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpGet("GetSubCategories/{categoryId}")]
        public async Task<IReadOnlyList<SubCategoryToReturnDto>> GetSubCategoriesByCategoryId(int categoryId)
        {
            var category = await _categoryRepo.GetById(categoryId);
            if (category == null)
                return null;
            var subCategories = await _subCategoryRepo.GetAll();
            subCategories = subCategories.Where(x => x.CategoryId == categoryId).ToList();
            var subCategoriesToReturn = _mapper.Map<IReadOnlyList<SubCategory>, IReadOnlyList<SubCategoryToReturnDto>>(subCategories);
            return subCategoriesToReturn;
            throw new Exception("Error happen when get SubCategories, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");

        }

        [HttpPost("AddSubCategory")]
        [Authorize]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategory entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _subCategoryRepo.GetById(entity.Id) != null)
                return BadRequest("This SubCategory is exist");

            if (await _categoryRepo.GetById(entity.CategoryId) == null)
                return BadRequest(@"You Can't add SubCategory to not exist Category!");

            var subCategory = new SubCategory()
            {
                Name = entity.Name,
                CategoryId = entity.CategoryId
            };

            if (await _subCategoryRepo.Add(subCategory))
                return Ok();
            throw new Exception("Error happen when add Sub Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpPut("updateNameSubCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNameSubCategory(int id, SubCategoryForUpdateDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _subCategoryRepo.GetById(id) == null)
                return BadRequest("This SubCategory is not exist");

            var subCategory = await _subCategoryRepo.GetById(id);

            if (await _categoryRepo.GetById(subCategory.CategoryId) == null)
                return BadRequest(@"You Can't update SubCategory to not exist Category!");

            subCategory.Name = entity.Name;
            if (await _subCategoryRepo.Update(subCategory))
                return Ok();
            throw new Exception("Error happen when update Sub Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpDelete("DeleteSubCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var subCategory = await _subCategoryRepo.GetById(id);
            if (subCategory == null)
                return BadRequest("Please add SubCategory exist!");
            if (await _subCategoryRepo.Delete(subCategory.Id))
                return Ok();
            throw new Exception("Error happen when delete Sub Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }
    }
}