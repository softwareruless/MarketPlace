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
        private static readonly string pathProj = Environment.CurrentDirectory;

        public static async Task<PhotosResponseModel> UploadPhotos(PhotoAddModel model)
        {
            string path = "";

            try
            {
                if (model.Photos.Where(x => x.Length > 0).Count() > model.Photos.Count())
                {
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

        #region Seller
        public static async Task<UploadPhotoResponseModel> UploadSellerProfilePhoto(IFormFile photo)
        {
            string path = "";

            try
            {
                if (photo.Length > 0)
                {
                    path = Path.GetFullPath(pathProj + "\\wwwroot\\SellerProfilePhotos");
                    var rnd = new Random();

                    var random = RandomHelper.CreateRandomDigits(rnd, 15);
                    var photoName = random + "-sellerProfile-" + photo.FileName.Trim().Replace(" ", "_");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, photoName), FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                    }

                    return new UploadPhotoResponseModel()
                    {
                        Success = true,
                        Path = photoName
                    };
                }
                else
                {
                    return new UploadPhotoResponseModel()
                    {
                        Success = false,
                    };
                }
            }
            catch (Exception ex)
            {
                return new UploadPhotoResponseModel()
                {
                    Success = false,
                };
            }
        }

        public static async Task<UploadPhotoResponseModel> UploadSellerCover(IFormFile photo)
        {
            string path = "";

            try
            {
                if (photo.Length > 0)
                {
                    path = Path.GetFullPath(pathProj + "\\wwwroot\\SellerCover");
                    var rnd = new Random();

                    var random = RandomHelper.CreateRandomDigits(rnd, 15);
                    var photoName = random + "-sellerCover-" + photo.FileName.Trim().Replace(" ", "_");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, photoName), FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                    }

                    return new UploadPhotoResponseModel()
                    {
                        Success = true,
                        Path = photoName
                    };
                }
                else
                {
                    return new UploadPhotoResponseModel()
                    {
                        Success = false,
                    };
                }
            }
            catch (Exception ex)
            {
                return new UploadPhotoResponseModel()
                {
                    Success = false,
                };
            }
        }


        public static async Task<DocumentsResponseModel> UploadDocuments(DocumentsAddModel model)
        {
            string path = "";

            try
            {
                if (model.Documents.Where(x => x.Document.Length > 0).Count() > model.Documents.Count())
                {
                    path = Path.GetFullPath(pathProj + "\\wwwroot\\SellerDocument");
                    var rnd = new Random();

                    var docs = new List<DocumentResponse>() { };

                    foreach (var document in model.Documents)
                    {
                        var random = RandomHelper.CreateRandomDigits(rnd, 15);
                        var docName = random + "-" + document.Document.FileName.Trim().Replace(" ", "_");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (var fileStream = new FileStream(Path.Combine(path, docName), FileMode.Create))
                        {
                            await document.Document.CopyToAsync(fileStream);
                        }

                        docs.Add(new DocumentResponse()
                        {
                            DocType = document.DocType,
                            Path = docName
                        });
                    }

                    if (docs.Count != model.Documents.Count())
                    {
                        return new DocumentsResponseModel()
                        {
                            Success = false,
                            Message = _configuration["FileUploadService.ErrorOcurred"]
                        };
                    }

                    return new DocumentsResponseModel()
                    {
                        Success = true,
                        Docs = docs
                    };
                }
                else
                {
                    return new DocumentsResponseModel()
                    {
                        Success = false,
                        Message = _configuration["FileUploadService.NullFile"]
                    };
                }
            }
            catch (Exception ex)
            {
                return new DocumentsResponseModel()
                {
                    Success = false,
                    Message = _configuration["FileUploadService.ErrorOcurred"]
                };
            }
        }

        #endregion

        #region delete
        public static void DeletePhoto(string PhotoPath, string directory)
        {
            string path = "";

            path = Path.Combine(Path.GetFullPath(pathProj + "\\wwwroot\\" + directory), PhotoPath);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }

        public static void DeletePhotos(DeletePhotosModel model, string directory)
        {
            string path = "";

            path = Path.GetFullPath(pathProj + "\\wwwroot\\" + directory);

            foreach (var photoPath in model.Paths)
            {

                var combinedPath = Path.Combine(path, photoPath);

                if (File.Exists(combinedPath))
                {
                    File.Delete(combinedPath);
                }
            }

        }
        #endregion

    }

    public class DeletePhotosModel
    {
        public List<string> Paths { get; set; }
    }

    public class UploadPhotoResponseModel
    {
        public bool Success { get; set; }
        public string Path { get; set; }
    }
}
