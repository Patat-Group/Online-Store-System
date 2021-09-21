using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Category;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category, int> _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryController(IGenericRepository<Category, int> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<CategoryReturnDto>> GetAll()
        {
            var categories = await _categoryRepo.GetAll();
            var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryReturnDto>>(categories);
            return data.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReturnDto>> GetById(int id)
        {
            var category = await _categoryRepo.GetById(id);
            var data = _mapper.Map<Category, CategoryReturnDto>(category);
            if (category != null) return Ok(data);
            throw new Exception("Error happen when get Category, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

    }
}