using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Service.Interfaces;
using MarketPlace.Utilities.TokenHelper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarketPlace.Service
{
    public class LoginService : ILoginService
    {
        private readonly MarketPlaceDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public LoginService(MarketPlaceDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<UserTokenResponseModel> Authenticate(UserLoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && !u.IsDeleted && u.EmailConfirmed);

            if (user != null)
            {
                var result = _signInManager.PasswordSignInAsync(user, model.Password, false, true).Result;

                if (result.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                                //new Claim(ClaimTypes.Name, user.Id.ToString()),
                                 new Claim(ClaimTypes.Name, user.UserName),
                                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 new Claim(ClaimTypes.Email,user.Email),
                                 (role.Any())?new Claim(ClaimTypes.Role, role[0]):null,
                        }),
                        Expires = DateTime.Now.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.AccessToken = tokenHandler.WriteToken(token);
                    user.RefreshToken = RefreshToken.CreateRefreshToken();
                    user.AccessTokenEndDate = DateTime.Now.AddDays(7);
                    await _userManager.UpdateAsync(user);

                    return new UserTokenResponseModel
                    {
                        Success = true,
                        Message = string.Empty,
                        Data = new UserTokenModel
                        {
                            AccessToken = user.AccessToken,
                            RefreshToken = user.RefreshToken,
                            Expiration = tokenDescriptor.Expires.Value,
                            FullName = user.FullName,
                            UserId = user.Id,
                            Role = role.Any() ? role[0] : string.Empty
                        }
                    };
                }
                else
                {
                    return new UserTokenResponseModel
                    {
                        Success = false,
                        Message = _configuration["LoginService.Authenticate"]
                    };
                }
            }
            else
            {
                return new UserTokenResponseModel
                {
                    Success = false,
                    Message = _configuration["LoginService.Authenticate"]
                };
            }
        }

        public async Task<ResponseModel> LogOut()
        {
            await _signInManager.SignOutAsync();
            return new ResponseModel
            {
                Success = true,
                Message = string.Empty,
            };
        }

        public async Task<UserTokenResponseModel> TokenRefresh(string refreshToken)
        {
            if (_userManager.Users.Any(x => x.RefreshToken == refreshToken))
            {
                User user = _userManager.Users.First(x => x.RefreshToken == refreshToken);

                if (user != null)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                                //new Claim(ClaimTypes.Name, user.Id.ToString()),
                                 new Claim(ClaimTypes.Name, user.UserName),
                                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 new Claim(ClaimTypes.Email,user.Email),
                                 (role.Any())?new Claim(ClaimTypes.Role, role[0]):null,
                        }),
                        Expires = DateTime.Now.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.AccessToken = tokenHandler.WriteToken(token);
                    user.RefreshToken = RefreshToken.CreateRefreshToken();
                    user.AccessTokenEndDate = DateTime.Now.AddDays(7);
                    await _userManager.UpdateAsync(user);

                    return new UserTokenResponseModel
                    {
                        Success = true,
                        Message = string.Empty,
                        Data = new UserTokenModel
                        {
                            AccessToken = user.AccessToken,
                            RefreshToken = user.RefreshToken,
                            Expiration = tokenDescriptor.Expires.Value,
                            FullName = user.FullName,
                            UserId = user.Id,
                        }
                    };
                }

                else
                {
                    return new UserTokenResponseModel
                    {
                        Success = false,
                        Message = _configuration["LoginService.TokenRefresh"]
                    };
                }
            }
            else
            {
                return new UserTokenResponseModel
                {
                    Success = false,
                    Message = _configuration["LoginService.TokenRefreshUser"]
                };
            }
        }

        public async Task<UserTokenResponseModel> AdminLogin(UserLoginModel model)
        {
            var user = _userManager.FindByEmailAsync(model.Email).Result;

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user); //User role?

                if (roles.Any())
                {
                    if (roles.Contains("Admin"))
                    {
                        var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;

                        if (result.Succeeded)
                        {
                            //var role = await _userManager.GetRolesAsync(user);
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                 new Claim(ClaimTypes.Name, user.UserName),
                                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 new Claim(ClaimTypes.Email,user.Email),
                                 (roles.Any())?new Claim(ClaimTypes.Role, roles[0]):null,
                                }),
                                Expires = DateTime.Now.AddDays(7),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            user.AccessToken = tokenHandler.WriteToken(token);
                            user.RefreshToken = RefreshToken.CreateRefreshToken();
                            user.AccessTokenEndDate = DateTime.Now.AddDays(7);
                            user.FullName = user.FullName;
                            await _userManager.UpdateAsync(user);
                            return new UserTokenResponseModel
                            {
                                Success = true,
                                Message = string.Empty,
                                Data = new UserTokenModel
                                {
                                    FullName = user.FullName,
                                    AccessToken = user.AccessToken,
                                    RefreshToken = user.RefreshToken,
                                    Expiration = tokenDescriptor.Expires.Value,
                                    UserId = user.Id,
                                    Role = roles[0]
                                }
                            };
                        }
                        else
                        {
                            return new UserTokenResponseModel
                            {
                                Success = false,
                                Message = _configuration["LoginService.AdminLogin"]
                            };
                        }
                    }
                    else
                    {
                        return new UserTokenResponseModel
                        {
                            Success = false,
                            Message = _configuration["LoginService.AdminLoginAdminCheck"]
                        };
                    }
                }
                else
                {
                    return new UserTokenResponseModel
                    {
                        Success = false,
                        Message = _configuration["LoginService.AdminLoginAdminCheck"]
                    };
                }
            }
            else
            {
                return new UserTokenResponseModel
                {
                    Success = false,
                    Message = _configuration["LoginService.AdminLogin"]
                };
            }
        }

        public async Task<ResponseModel> CreateRoleAsync(User user)
        {

            var Role = new Role()
            {
                Name = "Admin"
            };

            var roleResult = _roleManager.CreateAsync(Role);

            while (roleResult.Status == TaskStatus.WaitingForActivation)
            {
                Task.Delay(1000);

            }

            var result = _userManager.AddToRoleAsync(user, "Admin");

            while (result.Status == TaskStatus.WaitingForActivation)
            {
                Task.Delay(1000);

            }

            return new ResponseModel()
            {
                Success = true,
                Message = "",
            };
        }

    }
}
