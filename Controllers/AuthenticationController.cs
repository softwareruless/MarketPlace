using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Service;
using MarketPlace.Utilities.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    public class AuthenticationController : ApiKeyAuthBaseController
    {
        private readonly ILoginService _authenticationService;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(ILoginService authenticationService, UserManager<User> userManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<UserTokenResponseModel>> Login(UserLoginModel model)
        {
            var response = await _authenticationService.Authenticate(model);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<ResponseModel>> LogOut()
        {
            var response = await _authenticationService.LogOut();
            return Ok(response);
        }

        //[NotFoundFilter]
        [HttpPost]
        //[HttpPost("refreshToken")]

        public async Task<ActionResult<UserTokenResponseModel>> TokenRefresh(RefreshTokenModel model)
        {
            var response = await _authenticationService.TokenRefresh(model.Token);
            return Ok(response);
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<UserTokenResponseModel>> AdminLogin(UserLoginModel model)
        {
            var response = await _authenticationService.AdminLogin(model);
            return Ok(response);
        }

        [HttpPost]
        public string CreateNewToken()
        {
            const string iss = "62QM29578N"; // your account's team ID found in the dev portal
            const string aud = "https://appleid.apple.com";
            const string sub = "com.scottbrady91.authdemo.service"; // same as client_id
            var now = DateTime.UtcNow;

            // contents of your .p8 file
            const string privateKey = "MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgnbfHJQO9feC7yKOenScNctvHUP+Hp3AdOKnjUC3Ee9GgCgYIKoZIzj0DAQehRANCAATMgckuqQ1MhKALhLT/CA9lZrLA+VqTW/iIJ9GKimtC2GP02hCc5Vac8WuN6YjynF3JPWKTYjg2zqex5Sdn9Wj+";
            var ecdsa = ECDsa.Create();
            ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = iss,
                Audience = aud,
                Claims = new Dictionary<string, object> { { "sub", sub } },
                Expires = now.AddMinutes(5), // expiry can be a maximum of 6 months - generate one per request or re-use until expiration
                IssuedAt = now,
                NotBefore = now,
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256)
            });
        }

        [NonAction]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = _authenticationService.CreateRoleAsync(user);
            return Ok(result);
        }
    }
}
