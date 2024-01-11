using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Service;
using MarketPlace.Service.Interfaces;
using MarketPlace.Utilities.Filter;
using System.Data;

namespace MarketPlace.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public BlogController(IBlogService blogService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _blogService = blogService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        //[ValidationFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<ResponseModel>> Post([FromForm] BlogAddModel blogAddModel)
        {
            var result = await _blogService.Create(blogAddModel);
            return Ok(result);
        }

        [HttpPost]
        [ValidationFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<ResponseModel>> Update([FromForm] BlogUpdateModel blogUpdateModel)
        {
            var result = await _blogService.Update(blogUpdateModel);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidationFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<ResponseModel> Delete(int id)
        {
            var result = _blogService.Delete(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidationFilter]
        public ActionResult<ResponseModel> GetBlog(int id)
        {
            var result = _blogService.GetBlog(id);
            return Ok(result);
        }

        [HttpPost]
        [ValidationFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<ResponseModel> GetBlogsAdmin()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();//pagesize
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var result = _blogService.GetBlogsAdmin(sortColumn, sortColumnDirection, searchValue, recordsTotal, pageSize, skip, draw);
            return Ok(result);
        }

        [HttpPost]
        [ValidationFilter]
        public ActionResult<ResponseModel> GetBlogs(PagingParams model)
        {
            var result = _blogService.GetBlogs(model);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<ResponseModel> GetFeaturedBlogs()
        {
            var result = _blogService.GetFeaturedBlogs();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseModel> CreateTag([FromForm] TagModel model)
        {
            var result = _blogService.CreateTag(model);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseModel> UpdateTag([FromForm] TagModel model)
        {
            var result = _blogService.UpdateTag(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<ResponseModel> DeleteTag(int id)
        {
            var result = _blogService.DeleteTag(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidationFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<ResponseModel> GetTag(int id)
        {
            var result = _blogService.GetTag(id);
            return Ok(result);
        }

        [HttpPost]
        [ValidationFilter]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<ResponseModel> GetTagsAdmin()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();//pagesize
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var result = _blogService.GetTagsAdmin(sortColumn, sortColumnDirection, searchValue, recordsTotal, pageSize, skip, draw);
            return Ok(result);
        }

        [HttpGet]
        [ValidationFilter]
        public ActionResult<ResponseModel> GetTags()
        {
            var result = _blogService.GetTags();
            return Ok(result);
        }
    }
}
