using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CategoryDtos;
using API.DTOs.SubCategoryDtos;
using AutoMapper;
using Core.Entities;
using Interfaces.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category, int> _categoryRepo;
        private readonly IGenericRepository<SubCategory, int> _subCategoryRepo;
        private readonly IMapper _mapper;

        public CategoryController(IGenericRepository<Category, int> categoryRepo,
            IGenericRepository<SubCategory, int> subCategoryRepo , IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<CategoryToReturnDto>> GetAll()
        {
             var categories = await _categoryRepo.GetALl();
             var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories);
             return data.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryToReturnDto>> GetById(int id)
        {
            var category = await _categoryRepo.GetById(id);
            var data = _mapper.Map<Category, CategoryToReturnDto>(category);
            if (category != null) return Ok(data);
            return BadRequest("This Category is not exist.");
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (await _categoryRepo.GetById(category.Id) != null)
                return BadRequest("This category is exist.");

            var categoryForAdd = new Category()
            {
                Name = category.Name
            };

            if (await _categoryRepo.Add(categoryForAdd))
                return Ok();
            return BadRequest("Error happen when add Category.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category entity)
        {
            var category = await _categoryRepo.GetById(id);
            if (category == null)
                return BadRequest("This category is not exist!");
            category.Name = entity.Name;
            if (await _categoryRepo.Update(category))
                return Ok();
            return BadRequest("Error happen when update Category.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.GetById(id);
            if (category == null)
                return BadRequest("Please check Category exist!");
            
            if (await _categoryRepo.Delete(id))
                return Ok();
            return BadRequest("Error happen when delete Category.");
        }

        [HttpGet("GetSubCategories/{categoryId}")]
        public async Task<IReadOnlyList<SubCategoryToReturnDto>> GetSubCategoriesByCategoryId(int categoryId)
        {
            var category = await _categoryRepo.GetById(categoryId);
            if (category == null)
                return null;
            var subCategories = await _subCategoryRepo.GetALl();
            subCategories = subCategories.Where(x => x.CategoryId == categoryId).ToList();
            var subCategoriesToReturn = _mapper.Map<IReadOnlyList<SubCategory>,IReadOnlyList<SubCategoryToReturnDto>>(subCategories);
            return subCategoriesToReturn;
        }

        [HttpPost("AddSubCategory")]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategory entity)
        {
            if (await _subCategoryRepo.GetById(entity.Id) !=null)
                return BadRequest("This SubCategory is exist");
            
            if (await _categoryRepo.GetById(entity.CategoryId) ==null)
                return BadRequest(@"You Can't add SubCategory to not exist Category!");
            
            var subCategory =new SubCategory()
            {
                Name = entity.Name,
                CategoryId = entity.CategoryId
            };
            
            if (await _subCategoryRepo.Add(subCategory))
                return Ok();
            return BadRequest("Error happen when add SubCategory.");
        }

        [HttpPut("updateNameSubCategory/{id}")]
        public async Task<IActionResult> UpdateNameSubCategory(int id, SubCategoryForUpdateDto entity)
        {
            if (await _subCategoryRepo.GetById(id) ==null)
                return BadRequest("This SubCategory is not exist");
            
            var subCategory = await _subCategoryRepo.GetById(id);
            
            if (await _categoryRepo.GetById(subCategory.CategoryId) ==null)
                return BadRequest(@"You Can't update SubCategory to not exist Category!");
            
            subCategory.Name = entity.Name;
            if (await _subCategoryRepo.Update(subCategory))
                return Ok();
            return BadRequest("Error happen when update name SubCategory.");
        }

        [HttpDelete("DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var subCategory = await _subCategoryRepo.GetById(id);
            if (subCategory == null)
                return BadRequest("Please add SubCategory exist!");
            if (await _subCategoryRepo.Delete(subCategory.Id))
                return Ok();
            return BadRequest("Error happen when delete SubCategory.");
        }
    }
}