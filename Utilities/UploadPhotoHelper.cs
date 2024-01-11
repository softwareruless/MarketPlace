using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MarketPlace.Data.Entities;
using MarketPlace.Data;
using MarketPlace.Data.Model;

namespace MarketPlace.Utilities
{
    public static class UploadPhotoHelper
    {
        private static readonly IConfiguration _configuration;

        public static async Task<UploadBlogResponseModel> UploadBlog(BlogAddModel blogAddModel)
        {
            string path = "";

            try
            {
                if (blogAddModel.Photo.Length > 0)
                {
                    var pathProj = Environment.CurrentDirectory;
                    path = Path.GetFullPath(pathProj + "\\wwwroot\\BlogPhotos");
                    var rnd = new Random();
                    var random = RandomHelper.CreateRandomDigits(rnd, 15);
                    var photoName = random + "-" + blogAddModel.Photo.FileName.Trim().Replace(" ", "_");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, photoName), FileMode.Create))
                    {
                        await blogAddModel.Photo.CopyToAsync(fileStream);
                    }

                    return new UploadBlogResponseModel()
                    {
                        Success = true,
                        Url = photoName
                    };
                }
                else
                {
                    return new UploadBlogResponseModel()
                    {
                        Success = false,
                        Message = _configuration["FileUploadService.NullFile"]
                    };
                }
            }
            catch (Exception ex)
            {
                return new UploadBlogResponseModel()
                {
                    Success = false,
                    //Message = _configuration["FileUploadService.ErrorOcurred"]
                    Message = ex.Message
                };
            }
        }

        public static async Task<UploadBlogResponseModel> UpdateBlog(BlogUpdateModel blogUpdateModel)
        {
            string path = "";

            try
            {
                if (blogUpdateModel.Photo.Length > 0)
                {
                    var pathProj = Environment.CurrentDirectory;
                    path = Path.GetFullPath(pathProj + "\\wwwroot\\BlogPhotos");
                    var rnd = new Random();
                    var random = RandomHelper.CreateRandomDigits(rnd, 15);
                    var photoName = random + "-" + blogUpdateModel.Photo.FileName.Trim().Replace(" ", "_");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, photoName), FileMode.Create))
                    {
                        await blogUpdateModel.Photo.CopyToAsync(fileStream);
                    }

                    return new UploadBlogResponseModel()
                    {
                        Success = true,
                        Url = photoName
                    };
                }
                else
                {
                    return new UploadBlogResponseModel()
                    {
                        Success = false,
                        Message = _configuration["FileUploadService.NullFile"]
                    };
                }
            }
            catch (Exception ex)
            {
                return new UploadBlogResponseModel()
                {
                    Success = false,
                    //Message = _configuration["FileUploadService.ErrorOcurred"]
                    Message = ex.Message
                };
            }
        }


    }
}
