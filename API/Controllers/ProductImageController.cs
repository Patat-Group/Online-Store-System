using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.DTOs.ProductImagesDtos;
using AutoMapper;
using Core.Entities;
using Interfaces.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{productId}/Image")]
    public class ProductImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IGenericRepository<Product, int> _productRepo;
        private readonly IMapper _mapper;

        public ProductImageController(IImageRepository imageRepository,
            IGenericRepository<Product, int> productRepo, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<ProductImagesForReturnDto>> GetImagesForProduct(int productId)
        {
            var images = await _imageRepository.GetAllImagesForProduct(productId);
            var imagesForReturn =
                _mapper.Map<IReadOnlyList<ProductImage>, IReadOnlyList<ProductImagesForReturnDto>>(images);
            return imagesForReturn;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductImagesForReturnDto>> GetImage(int id)
        {
            var image = await _imageRepository.GetImageById(id);
            var imageForReturn = _mapper.Map<ProductImagesForReturnDto>(image);
            return imageForReturn;
        }
        
   
        [HttpPost]
        public async Task<IActionResult> AddImage(int productId, [FromForm] ProductImageForAddDto entity)
        {
            var product = await _productRepo.GetById(productId);
            if (product == null)
                return BadRequest("You can't add image for product not exist.");
            
            var images = await _imageRepository.GetAllImagesForProduct(productId);
            if (images.Count == 4)
                    return BadRequest("Sorry, you can't add more than 4 images to your products.");
            
            var file = entity.File;
            if (file == null)
                return BadRequest("Please add photo to your product.");

            var path = Path.Combine("wwwroot/images/", file.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();
            
            entity.ImageUrl = path.Substring(7);
            entity.ProductId = product.Id;
            if (images.Count == 0)
                entity.IsMainPhoto = true;
            
            var imageForAdd = _mapper.Map<ProductImage>(entity);
            if (await _imageRepository.AddImage(imageForAdd))
                return Ok();
            
            return BadRequest("Error happen when add photo to your product.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SetImageMain(int id)
        {
            if (await _imageRepository.SetMainImage(id))
                return Ok();
            return BadRequest("Error when set image main.");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _imageRepository.GetImageById(id);
            
            if(System.IO.File.Exists("wwwroot" +image.ImageUrl))
                System.IO.File.Delete("wwwroot"+image.ImageUrl);
            else 
                return NotFound("Not Found Image in Server.");
            
            if (await _imageRepository.DeleteImage(id))
                return Ok();
            return BadRequest("Error when remove image.");
        }
    }
}