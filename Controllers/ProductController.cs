using System;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Service;
using MarketPlace.Utilities.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
	public class ProductController : AuthenticationBaseController
	{
        private readonly IProductService _productService;
        private readonly UserManager<User> _userManager;

        public ProductController(IProductService productService, UserManager<User> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> CreateProduct(ProductAddModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var response = _productService.CreateProduct(model,user.Id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> UpdateProduct(ProductUpdateModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var response = _productService.UpdateProduct(model, user.Id);
            return Ok(response);
        }

        [HttpPost("{ProductId}")]
        public async Task<ActionResult<ResponseModel>> DeleteProduct(int ProductId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var response = _productService.DeleteProduct(ProductId, user.Id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> AddPhoto(PhotoAddModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var response = await _productService.AddPhoto(model, user.Id);
            return Ok(response);
        }
        // Check this endpoint ProductService.cs : 241
        [HttpPost("{PhotoId}")]
        public async Task<ActionResult<ResponseModel>> DeletePhoto(int PhotoId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var response = _productService.DeletePhoto(PhotoId, user.Id);
            return Ok(response);
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public ActionResult<ProductDetailResponseModel> GetProduct(int productId)
        {
            var response = _productService.GetProduct(productId);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<DatatableModel<ProductDetail>> GetProducts(PagingParams pagingParams)
        {
            var response = _productService.GetProducts(pagingParams);
            return Ok(response);
        }

    }
}

