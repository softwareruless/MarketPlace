using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Service;
using MarketPlace.Utilities.Filter;
using System.Data;

namespace MarketPlace.Controllers
{
    public class UserController : ApiKeyAuthBaseController
    {

        private readonly UserService _userService;
        private readonly UserManager<User> _userManager;
        public static readonly object LockObjectVerify = new object();

        public UserController(UserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Post(UserAddModel userAddModel)
        {
            var result = await _userService.CreateUserAsync(userAddModel);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ValidationFilter]
        [HttpPost]
        public ActionResult<DatatableModel<UserDetail>> GetUsers()
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

            var result = _userService.GetUsers(sortColumn, sortColumnDirection, searchValue, recordsTotal, pageSize, skip, draw);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UserDetailResponseModel>> GetUserAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = _userService.GetUser(user);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> UpdateName(UpdateNameModel updateNameModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userService.UpdateName(updateNameModel, user);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> UpdateEMail(UpdateEmailModel updateEmailModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userService.UpdateEMail(updateEmailModel, user);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> UpdatePassword(UserPasswordChangeModel userPasswordChangeModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userService.UpdatePassword(userPasswordChangeModel, user);
            return Ok(result);
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> VerifyCode(VerifyCodeModel verifyCodeModel)
        {
            lock (LockObjectVerify)
            {
                var response = _userService.VerifyCode(verifyCodeModel).Result;
                return Ok(response);
            }
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PasswordReset(PasswordResetModel passwordResetModel)
        {
            lock (LockObjectVerify)
            {
                var response = _userService.PasswordReset(passwordResetModel).Result;
                return Ok(response);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<ResponseModel>> Archive()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userService.Archive(user);
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<ResponseModel>> SendVerificationCode(string email)
        {
            var response = _userService.SendVerificationCode(email);
            return Ok(response);

        }

        


    }
}
