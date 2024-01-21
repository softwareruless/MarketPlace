using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MarketPlace.Data.Entities;
using MarketPlace.Data;
using MarketPlace.Data.Model;
using Microsoft.AspNetCore.Hosting.Server;

namespace MarketPlace.Utilities
{
    public static class UploadPhotoHelper
    {
        private static readonly IConfiguration _configuration;

        public static async Task<PhotosResponseModel> UploadPhotos(PhotoAddModel model)
        {
            string path = "";

            try
            {
                if (model.Photos.Where(x => x.Length > 0).Count() > model.Photos.Count())
                {
                    var pathProj = Environment.CurrentDirectory;
                    path = Path.GetFullPath(pathProj + "\\wwwroot\\ProductPhotos");
                    var rnd = new Random();

                    var photoNames = new List<string>() { };

                    foreach (var photo in model.Photos)
                    {
                        var random = RandomHelper.CreateRandomDigits(rnd, 15);
                        var photoName = random + "-" + photo.FileName.Trim().Replace(" ", "_");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (var fileStream = new FileStream(Path.Combine(path, photoName), FileMode.Create))
                        {
                            await photo.CopyToAsync(fileStream);
                        }

                        photoNames.Add(photoName);
                    }

                    if (photoNames.Count != model.Photos.Count())
                    {
                        return new PhotosResponseModel()
                        {
                            Success = false,
                            Message = _configuration["FileUploadService.ErrorOcurred"]
                        };
                    }

                    return new PhotosResponseModel()
                    {
                        Success = true,
                        Names = photoNames
                    };
                }
                else
                {
                    return new PhotosResponseModel()
                    {
                        Success = false,
                        Message = _configuration["FileUploadService.NullFile"]
                    };
                }
            }
            catch (Exception ex)
            {
                return new PhotosResponseModel()
                {
                    Success = false,
                    Message = _configuration["FileUploadService.ErrorOcurred"]
                };
            }
        }

        public static void DeletePhoto(string PhotoPath)
        {
            string path = "";

            var pathProj = Environment.CurrentDirectory;
            path = Path.Combine(Path.GetFullPath(pathProj + "\\wwwroot\\ProductPhotos"), PhotoPath);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }

        public static void DeletePhotos(DeletePhotosModel model)
        {
            string path = "";

            var pathProj = Environment.CurrentDirectory;
            path = Path.GetFullPath(pathProj + "\\wwwroot\\ProductPhotos");

            foreach (var photoPath in model.Paths)
            {

                var combinedPath = Path.Combine(path, photoPath);

                if (File.Exists(combinedPath))
                {
                    File.Delete(combinedPath);
                }
            }

        }


        //public static async Task<UploadBlogResponseModel> UpdateBlog(BlogUpdateModel blogUpdateModel)
        //{
        //    string path = "";

        //    try
        //    {
        //        if (blogUpdateModel.Photo.Length > 0)
        //        {
        //            var pathProj = Environment.CurrentDirectory;
        //            path = Path.GetFullPath(pathProj + "\\wwwroot\\BlogPhotos");
        //            var rnd = new Random();
        //            var random = RandomHelper.CreateRandomDigits(rnd, 15);
        //            var photoName = random + "-" + blogUpdateModel.Photo.FileName.Trim().Replace(" ", "_");
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            using (var fileStream = new FileStream(Path.Combine(path, photoName), FileMode.Create))
        //            {
        //                await blogUpdateModel.Photo.CopyToAsync(fileStream);
        //            }

        //            return new UploadBlogResponseModel()
        //            {
        //                Success = true,
        //                Url = photoName
        //            };
        //        }
        //        else
        //        {
        //            return new UploadBlogResponseModel()
        //            {
        //                Success = false,
        //                Message = _configuration["FileUploadService.NullFile"]
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new UploadBlogResponseModel()
        //        {
        //            Success = false,
        //            //Message = _configuration["FileUploadService.ErrorOcurred"]
        //            Message = ex.Message
        //        };
        //    }
        //}


    }

    public class DeletePhotosModel
    {
        public List<string> Paths { get; set; }
    }
}
