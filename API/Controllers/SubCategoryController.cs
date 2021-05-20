using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.SubCategoryDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/subCategory")]
    public class SubCategoryController : ControllerBase
    {
        private readonly IGenericRepository<SubCategory, int> _subCategoryRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public SubCategoryController(IGenericRepository<SubCategory, int> subCategoryRepo,
             IUserRepository userRepo, IMapper mapper)
        {
            _subCategoryRepo = subCategoryRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<SubCategoryToReturnDto>> GetAll()
        {
            var subCategories = await _subCategoryRepo.GetAll();
            var data = _mapper.Map<IReadOnlyList<SubCategory>, IReadOnlyList<SubCategoryToReturnDto>>(subCategories);
            return data.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategoryToReturnDto>> GetById(int id)
        {
            var subCategory = await _subCategoryRepo.GetById(id);
            var data = _mapper.Map<SubCategory, SubCategoryToReturnDto>(subCategory);
            if (subCategory != null) return Ok(data);
            throw new Exception("Error happen when get SubCategory, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(SubCategoryForAddDto subCategory)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            if (await _subCategoryRepo.GetById(subCategory.Id) != null)
                return BadRequest("This Subcategory is exist.");

            var subCategoryForAdd = new SubCategory()
            {
                Name = subCategory.Name.ToLower()
            };

            if (await _subCategoryRepo.Add(subCategoryForAdd))
                return Ok();
            throw new Exception("Error happen when add SubCategory, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, SubCategoryForAddDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var subCategory = await _subCategoryRepo.GetById(id);
            if (subCategory == null)
                return BadRequest("This SubCategory is not exist!");

            subCategory.Name = entity.Name.ToLower();
            if (await _subCategoryRepo.Update(subCategory))
                return Ok();
            throw new Exception("Error happen when update SubCategory, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var category = await _subCategoryRepo.GetById(id);
            if (category == null)
                return BadRequest("Please check Category exist!");

            if (await _subCategoryRepo.Delete(id))
                return Ok();
            throw new Exception("Error happen when delete SubCategory, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }
    }
}