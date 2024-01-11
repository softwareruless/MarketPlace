using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Utilities;

using System.Linq.Dynamic.Core;

namespace MarketPlace.Service
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly MarketPlaceDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private bool isAdmin = false;

        public UserService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, MarketPlaceDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        //CreateUser
        public async Task<ResponseModel> CreateUserAsync(UserAddModel userAddModel)
        {
            var hasUser = _userManager.FindByEmailAsync(userAddModel.Email).Result;

            if (hasUser == null)
            {
                var user = _mapper.Map<User>(userAddModel);
                user.UserName = userAddModel.Email;
                user.CreatedDate = DateTime.Now;
                var result = await _userManager.CreateAsync(user, userAddModel.Password);

                if (result.Succeeded)
                    return SendVerificationCode(userAddModel.Email);
            }
            else if (hasUser.IsDeleted)
            {
                hasUser.IsDeleted = false;
                var UpdateUserResult = _userManager.UpdateAsync(hasUser).Result;

                if (UpdateUserResult.Succeeded)
                    return SendVerificationCode(userAddModel.Email);
            }
            else if (!hasUser.EmailConfirmed)
            {
                var oldUser = await _userManager.FindByEmailAsync(userAddModel.Email);
                var newHashedPassword = _userManager.PasswordHasher.HashPassword(oldUser, userAddModel.Password);
                oldUser.FullName = userAddModel.FullName;
                oldUser.PasswordHash = newHashedPassword;
                var result = await _userManager.UpdateAsync(oldUser);

                if (result.Succeeded)
                    return SendVerificationCode(userAddModel.Email);
            }

            //if not return
            return new ResponseModel()
            {
                Success = false,
                Message = _configuration["UserService.CreateUser"]
            };

        }

        public ResponseModel SendVerificationCode(string Mail)
        {
            var saveResult = 0;
            var sendmailResult = EmailHelper.SendVerificationMail(Mail);

            if (sendmailResult.Success)
            {
                var emailVerificationResult = _context.EmailVerification.FirstOrDefault(x => x.Email == Mail);

                if (emailVerificationResult != null)
                {
                    emailVerificationResult.Code = sendmailResult.Data;
                    emailVerificationResult.ExpireDate = DateTime.Now.AddMinutes(5);
                    emailVerificationResult.TryCount = 0;

                    _context.EmailVerification.Update(emailVerificationResult);
                    saveResult = _context.SaveChanges();
                }
                else
                {
                    var EmailVerification = new EmailVerification()
                    {
                        Code = sendmailResult.Data,
                        Email = Mail,
                        ExpireDate = DateTime.Now.AddMinutes(5),
                    };

                    _context.EmailVerification.Add(EmailVerification);
                    saveResult = _context.SaveChanges();
                }

                if (saveResult > 0)
                {
                    return new ResponseModel()
                    {
                        Success = true,
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        Success = false,
                        Message = _configuration["MailServices.SendVerificationMail"]
                    };
                }

            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = sendmailResult.Message
                };
            }

        }

        //GetUser
        public DatatableModel<UserDetail> GetUsers(string sortColumn, string sortColumnDirection, string searchValue, int recordsTotal, int pageSize, int skip, string draw)
        {
            var users = _context.Users.Select(x => new UserDetail()
            {
                FullName = x.FullName,
                Email = x.Email,
                CreatedDate = x.CreatedDateStr,
                EmailConfirmed = x.EmailConfirmed
            }).AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                users = users.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                users = users.Where(m =>
                (m.FullName.ToLower() ?? string.Empty).Contains(searchValue.ToLower()) ||
                (m.Email.ToLower() ?? string.Empty).Contains(searchValue.ToLower()));
            }

            recordsTotal = (users != null) ? users.Count() : 0;
            var data = users.Skip(skip).Take(pageSize).ToList();
            var jsonData = new DatatableModel<UserDetail> { draw = draw, recordsFiltered = data.Count(), recordsTotal = recordsTotal, data = data };

            return jsonData;
        }

        public UserDetailResponseModel GetUser(User User)
        {

            if (User != null)
            {
                return new UserDetailResponseModel()
                {
                    Success = true,
                    User = new UserDetail()
                    {
                        Email = User.Email,
                        FullName = User.FullName,
                    }
                };
            }

            return new UserDetailResponseModel()
            {
                Success = false,
                Message = _configuration["UserService.GetUser"]
            };
        }

        //Update
        public async Task<ResponseModel> UpdateName(UpdateNameModel updateNameModel, User olduser)
        {
            if (olduser != null)
            {
                olduser.FullName = updateNameModel.Name;
                var updatedUser = _mapper.Map<UpdateNameModel, User>(updateNameModel, olduser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded)
                {
                    return new ResponseModel
                    {
                        Success = true,
                        Message = string.Empty,
                    };
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.UpdateName"]
                    };
                }
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = _configuration["UserService.NotFound"]
                };
            }
        }

        public async Task<ResponseModel> UpdateEMail(UpdateEmailModel updateEmailModel, User olduser)
        {
            var response = VerifyCodeByUpdateEmail(new VerifyCodeModel { Code = updateEmailModel.Code, Email = updateEmailModel.NewEmail });
            if (response.Success == true)
            {
                olduser.Email = updateEmailModel.NewEmail;
                olduser.UserName = updateEmailModel.NewEmail;
                olduser.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(olduser);

                if (result.Succeeded)
                {
                    //Email güncellendikten sonra EmailCode tablosundaki eski mail ve code bilgisi silinebilir.
                    var isRemoved = RemoveEmailVerification(_context.EmailVerification.FirstOrDefault(e => e.Email == updateEmailModel.OldEmail));

                    if (isRemoved > 0)
                    {
                        return new ResponseModel
                        {
                            Success = true,
                            Message = string.Empty,
                        };
                    }
                    else
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = _configuration["UserService.UpdateEMail"]
                        };
                    }
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.UpdateEMailRemoved"]
                    };
                }

            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = _configuration["UserService.UpdateEMailNotVerify"]
                };
            }
        }

        public async Task<ResponseModel> UpdatePassword(UserPasswordChangeModel userPasswordChangeModel, User user)
        {
            if (user != null)
            {
                //Kullanıcı şifresini biliyor mu
                var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeModel.CurrentPassword);

                if (isVerified)
                {
                    //user.Password = changeDto.NewPassword;
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeModel.CurrentPassword, userPasswordChangeModel.NewPassword);

                    if (result.Succeeded)
                    {
                        //user'a ait önemli değişikliği belirt.
                        await _userManager.UpdateSecurityStampAsync(user);
                        //Kullanıcıya çıkış yaptır ve yeni şifresiyle girmesini sağla.
                        // _signInManager.SignOutAsync();
                        // _signInManager.PasswordSignInAsync(user, changeDto.NewPassword, true, false);
                        return new ResponseModel
                        {
                            Success = true,
                            Message = string.Empty,
                        };
                    }
                    else
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = _configuration["UserService.UpdatePassword"]
                        };
                    }
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.UpdatePasswordCurrentPassword"]
                    };
                }
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = _configuration["UserService.UpdatePasswordNotFound"]
                };
            }
        }

        //Delete
        public async Task<ResponseModel> Archive(User user)
        {
            if (user != null)
            {
                user.IsDeleted = true;
                user.EmailConfirmed = false;
                var UpdateUserResult = _userManager.UpdateAsync(user);

                if (UpdateUserResult.Result.Succeeded)
                {
                    return new ResponseModel
                    {
                        Success = true,
                        Message = string.Empty,
                    };
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.Archive"]
                    };
                }
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = _configuration["UserService.ArchiveNotFound"]
                };
            }
        }

        //VerifyCode
        public async Task<ResponseModel> VerifyCode(VerifyCodeModel verifyCodeModel)
        {
            var result = _context.EmailVerification.FirstOrDefault(e => e.Email == verifyCodeModel.Email && e.Code == verifyCodeModel.Code);

            if (result != null)
            {
                if (result.ExpireDate > DateTime.Now && result.TryCount < 5)
                {
                    var response = UpdateEmailConfirmed(verifyCodeModel.Email).Result;

                    result.TryCount += 1;

                    _context.EmailVerification.Update(result);
                    _context.SaveChanges();

                    if (response)
                    {
                        return new ResponseModel
                        {
                            Success = true,
                            Message = string.Empty
                        };
                    }
                    else
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = _configuration["UserService.VerifyCode"]
                        };
                    }

                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.VerifyCodeExpireDate"]
                    };
                }
            }
            else
            {
                var result1 = _context.EmailVerification.FirstOrDefault(e => e.Email == verifyCodeModel.Email && e.ExpireDate > DateTime.Now);

                result1.TryCount += 1;

                _context.EmailVerification.Update(result1);
                _context.SaveChanges();
            }

            return new ResponseModel
            {
                Success = false,
                Message = _configuration["UserService.VerifyCodeNotFound"]
            };
        }

        private async Task<bool> UpdateEmailConfirmed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                user.EmailConfirmed = true;
                var response = await _userManager.UpdateAsync(user);

                return response.Succeeded;
            }

            return false;
        }

        public ResponseModel VerifyCodeByUpdateEmail(VerifyCodeModel model)
        {
            var result = _context.EmailVerification.FirstOrDefault(e => e.Email == model.Email && e.Code == model.Code);

            if (result != null)
            {
                if (result.ExpireDate > DateTime.Now)
                {

                    return new ResponseModel
                    {
                        Success = true,
                        Message = string.Empty
                    };

                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.VerifyCodeByUpdateEmailExpireDate"]
                    };
                }
            }
            return new ResponseModel
            {
                Success = false,
                Message = _configuration["UserService.VerifyCodeByUpdateEmail"]
            };
        }

        //ForgotPassword
        public async Task<ResponseModel> PasswordReset(PasswordResetModel passwordResetModel)
        {
            var response = await VerifyCode(new VerifyCodeModel { Code = passwordResetModel.Code, Email = passwordResetModel.Email });
            if (response.Success == true)
            {
                var user = await _userManager.FindByEmailAsync(passwordResetModel.Email);
                if (user != null)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordResetModel.NewPassword);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return new ResponseModel
                        {
                            Success = true,
                            Message = string.Empty
                        };
                    }
                    else
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = _configuration["UserService.PasswordReset"]
                        };
                    }
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = _configuration["UserService.PasswordResetNotFound"]
                    };
                }
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = _configuration["UserService.PasswordResetVerifyCode"]
                };
            }
        }

        public async Task<bool> IsAdmin(User user)
        {
            var roles = await _userManager.GetRolesAsync(user); //User role?

            if (roles.Count > 0)
            {
                isAdmin = roles.Contains("Admin");
            }

            return isAdmin;
        }

        private int RemoveEmailVerification(EmailVerification emailVerification)
        {
            _context.EmailVerification.Remove(emailVerification);
            return _context.SaveChanges();
        }

        public ResponseModel SendContactMessage(ContactMessageAddModel model)
        {
            var message = _mapper.Map<ContactForm>(model);
            message.CreatedDate = DateTime.Now;
            _context.ContactForm.Add(message);

            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = string.Empty,
                };
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = _configuration["UserService.ContactMessage"]
                };
            }
        }

        public DatatableModel<ContactMessageModel> GetContactMessages(string sortColumn, string sortColumnDirection, string searchValue, int recordsTotal, int pageSize, int skip, string draw, string isAnswer)
        {
            var contacs = _context.ContactForm.OrderByDescending(x=>x.CreatedDate).Select(x => new ContactMessageModel()
            {
                Name = x.Name,
                Email = x.Email,
                Message = x.Message,
                Subject = x.Subject,
                CreatedDate = x.CreatedDateStr,
                IsAnswered = x.IsAnswered,
                Id = x.Id
            }).AsQueryable();

            if (isAnswer != "null")
            {
                contacs = contacs.Where(x => x.IsAnswered == Convert.ToBoolean(isAnswer));
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                contacs = contacs.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                contacs = contacs.Where(m =>
                (m.Email.ToLower() ?? string.Empty).Contains(searchValue.ToLower()) ||
                (m.Name.ToLower() ?? string.Empty).Contains(searchValue.ToLower()) ||
                (m.Subject.ToLower() ?? string.Empty).Contains(searchValue.ToLower()) ||
                (m.Message.ToLower() ?? string.Empty).Contains(searchValue.ToLower()));
            }

            recordsTotal = (contacs != null) ? contacs.Count() : 0;
            var data = contacs.Skip(skip).Take(pageSize).ToList();
            var jsonData = new DatatableModel<ContactMessageModel> { draw = draw, recordsFiltered = data.Count(), recordsTotal = recordsTotal, data = data };

            return jsonData;
        }

        public ResponseModel UpdateContactStatus(AnswerContactMessageModel model)
        {
            var contact = _context.ContactForm.FirstOrDefault(x => x.Id == model.Id);

            if (contact == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullTag"],
                };
            }

            contact.IsAnswered = model.IsAnswered;

            _context.ContactForm.Update(contact);

            var result = _context.SaveChanges();

            if (result > 0)
            {
                return new ResponseModel()
                {
                    Success = true
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["UserService.UpdateContact"]
                };
            }
        }
    }
}
