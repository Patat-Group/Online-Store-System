#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.FavoriteProductDtos;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/favorite")]
    public class FavoriteProductController : ControllerBase
    {
        private readonly IFavoriteProductRepository _favoriteRepo;
        private readonly IGenericRepository<Product, int> _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public FavoriteProductController(IFavoriteProductRepository favoriteRepo,
            IGenericRepository<Product, int> productRepo, IUserRepository userRepo, IMapper mapper)
        {
            _favoriteRepo = favoriteRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }
        [HttpGet("all")]
        public async Task<IReadOnlyList<FavoriteProductToReturnDto>> GetAllFavoritesWithPaging([FromQuery] ProductParams? productParams)
        {
            var favorites = await _favoriteRepo.GetAllWithPaging(productParams);
            var favoritesToReturn =
                _mapper.Map<IReadOnlyList<FavoriteProduct>, IReadOnlyList<FavoriteProductToReturnDto>>(favorites);
            return favoritesToReturn;
        }
        [HttpGet("myFavorite/all")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<FavoriteProductToReturnDto>>> GetAllFavoritesByUser([FromQuery] ProductParams? productParams)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            var favorites = await _favoriteRepo.GetAllByUserIdWithPaging(user.Id,productParams);
            var favoritesToReturn =
                _mapper.Map<IReadOnlyList<FavoriteProduct>, IReadOnlyList<FavoriteProductToReturnDto>>(favorites);
            return Ok(favoritesToReturn);
        }

        [HttpPut("product/{productId}")]
        [Authorize]
        public async Task<ActionResult> AddToFavorite(int productId)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            var product = await _productRepo.GetById(productId);
            if (product == null) return BadRequest("Bad Product Id");
            var favoriteProduct = await _favoriteRepo.Get(user.Id, productId);
            if (favoriteProduct!= null) return BadRequest("Already In Favorites");
            var newFavoriteProduct = new FavoriteProduct
            {
                UserId = user.Id,
                ProductId = productId
            };
            var result = await _favoriteRepo.Add(newFavoriteProduct);
            if (result)
                return Ok("Add To Favorite Succeeded");
            return BadRequest("Error Happen When adding to Favorites");
        }
        [HttpDelete("product/{productId}")]
        [Authorize]
        public async Task<ActionResult> DeleteFromFavorite(int productId)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            var product = await _productRepo.GetById(productId);
            if (product == null) return BadRequest("Bad Product Id");
            var favoriteProduct = await _favoriteRepo.Get(user.Id, productId);
            if (favoriteProduct== null) return BadRequest("this product isn't favorited by this user");
            var result = await _favoriteRepo.Delete(favoriteProduct);
            if (result)
                return Ok("Deleting From Favorite Succeeded");
            return BadRequest("Error Happen When Deleting From Favorite");
        }
        [HttpPost("product/{productId}")]
        [Authorize]
        public async Task<ActionResult> CheckIfFavoritedByUser(int productId)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            var product = await _productRepo.GetById(productId);
            if (product == null) return BadRequest("Bad Product Id");
            var favoriteProduct = await _favoriteRepo.Get(user.Id, productId);
            return Ok(favoriteProduct== null ? "false" : "True");
        }
    }
}