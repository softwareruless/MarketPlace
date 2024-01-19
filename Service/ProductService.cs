using AutoMapper;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Service
{
    public class ProductService : IProductService
    {
        private readonly MarketPlaceDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProductService(MarketPlaceDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public ResponseModel CreateProduct(ProductAddModel model, int UserId)
        {
            var sellerResult = GetSellerIdWithUserId(UserId);

            if (!sellerResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.SellerNotFound"]
                };
            }

            Product product = _mapper.Map<Product>(model);

            product.SellerId = sellerResult.Seller.Id;

            _context.Product.Add(product);
            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.CreateProduct"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };
        }

        public ResponseModel UpdateProduct(ProductUpdateModel model, int UserId)
        {
            var seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted
            && x.UserId == UserId
            && x.Status == Data.Enums.Status.Approved);

            if (seller == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.SellerNotFound"]
                };
            }

            Product product = _context.Product.FirstOrDefault(x => x.SellerId == seller.Id && x.Id == model.Id);

            product.Name = product.Name;
            product.Description = product.Description;
            product.BrandId = product.BrandId;
            product.CategoryId = product.CategoryId;

            product.UpdatedDate = DateTime.Now;
            product.UpdatedBy = UserId;

            _context.Product.Update(product);
            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.UpdateProduct"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };
        }

        public ResponseModel AddPhoto(PhotoAddModel model, int UserId)
        {
            var sellerAndProductResult = GetSellerIdWithUserIdAndProductId(UserId, model.ProductId);

            if (!sellerAndProductResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.SellerNotFound"]
                };
            }

            var photoUploadResult = UploadPhotoHelper.UploadPhotos(model).Result;

            if (!photoUploadResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = photoUploadResult.Message
                };
            }

            var photos = photoUploadResult.Names.Select(x => new ProductPhoto()
            {
                ProductId = sellerAndProductResult.Product.Id,
                Path = x
            }).ToList();

            _context.ProductPhoto.AddRange(photos);
            var result = _context.SaveChanges();


            if (!(result >= photos.Count()))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.AddPhotos"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };
        }

        public ResponseModel DeletePhoto(int PhotoId,int UserId)
        {
            var verifyProductAndSellerResult = GetSellerIdWithUserIdAndPPhotoId(UserId, PhotoId);

            if (!verifyProductAndSellerResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration[""]
                };
            }

            _context.ProductPhoto.Remove(verifyProductAndSellerResult.PPhoto);
            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.DeleteProductPhoto"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };
        }

        public ProductDetailResponseModel GetProduct(int productId)
        {
            var product = _context.Product.Where(x => x.Id == productId).Select(x => new ProductDetail()
            {
                SellerId = x.SellerId,

                Name = x.Name,
                Description = x.Description,

                BrandId = x.BrandId,
                CategoryId = x.CategoryId
            }).FirstOrDefault();


            if (product == null)
            {
                return new ProductDetailResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.ProductNotFound"]
                };
            }

            return new ProductDetailResponseModel()
            {
                Success = true,
                Product = product
            };
        }

        public ResponseModel DeleteProduct(int productId, int UserId)
        {
            var verifySellerAndProductResult = GetSellerIdWithUserIdAndProductId(UserId, productId);

            if (!verifySellerAndProductResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.SellerNotFound"]
                };
            }

            var product = _context.Product.FirstOrDefault(x => x.Id == verifySellerAndProductResult.Product.Id);

            _context.Product.Remove(product);
            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["ProductService.DeleteProduct"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };

        }

        public DatatableModel<ProductDetail> GetProducts(PagingParams pagingParams)
        {
            var products = _context.Product.Where(x => !x.IsDeleted).AsQueryable();

            if (!String.IsNullOrEmpty(pagingParams.searchValue))
            {
                products = products.Where(x => x.Name.ToLower() == pagingParams.searchValue);
            }

            var filteredProducts = products.
            Select(x => new ProductDetail()
            {
                SellerId = x.SellerId,

                Name = x.Name,
                Description = x.Description,

                BrandId = x.BrandId,
                CategoryId = x.CategoryId
            }).ToList();

            pagingParams.recordsTotal = (filteredProducts != null) ? filteredProducts.Count() : 0;
            var data = filteredProducts.Skip(pagingParams.skip).Take(pagingParams.pageSize).ToList();
            var jsonData = new DatatableModel<ProductDetail> { draw = pagingParams.draw, recordsFiltered = data.Count(), recordsTotal = pagingParams.recordsTotal, data = data };

            return jsonData;
        }

        #region Helper Functions
        private SellerVerifyResult GetSellerIdWithUserId(int UserId)
        {
            var seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted
            && x.UserId == UserId
            && x.Status == Data.Enums.Status.Approved);

            if (seller == null)
            {
                return new SellerVerifyResult()
                {
                    Success = false
                };
            }

            return new SellerVerifyResult()
            {
                Success = true,
                Seller = seller
            };
        }

        private ProductVerifyResult GetSellerIdWithUserIdAndProductId(int UserId, int ProductId)
        {
            var seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted
            && x.UserId == UserId
            && x.Status == Data.Enums.Status.Approved);

            var product = _context.Product.FirstOrDefault(x => !x.IsDeleted
            && x.SellerId == seller.Id
            && x.Id == ProductId);

            if (product == null)
            {
                return new ProductVerifyResult()
                {
                    Success = false
                };
            }

            return new ProductVerifyResult()
            {
                Success = true,
                Seller = seller,
                Product = product
            };
        }

        private ProductPhotoVerifyResult GetSellerIdWithUserIdAndPPhotoId(int UserId, int PPhotoId)
        {
            var seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted
            && x.UserId == UserId
            && x.Status == Data.Enums.Status.Approved);

            var pphoto = _context.ProductPhoto.Include(x=>x.Product).FirstOrDefault(x => !x.IsDeleted);

            if (pphoto == null || pphoto.Product.SellerId != seller.Id)
            {
                return new ProductPhotoVerifyResult()
                {
                    Success = false
                };
            }

            return new ProductPhotoVerifyResult()
            {
                Success = true,
                Seller = seller,
                Product = pphoto.Product,
                PPhoto = pphoto
            };
        }
        #endregion
    }
}

