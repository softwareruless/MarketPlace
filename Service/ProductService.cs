using System;
using System.Reflection.Metadata;
using AutoMapper;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using MarketPlace.Utilities;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Service
{
    public class ProductService
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

            product.SellerId = sellerResult.SellerId;

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

            product.Name= product.Name;
            product.Description = product.Description;
            product.BrandId = product.BrandId;
            product.CategoryId = product.CategoryId;

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

        public ResponseModel AddPhoto(PhotoAddModel model,int UserId)
        {
            var sellerAndProductResult = GetSellerIdWithUserIdAndProductId(UserId,model.ProductId);

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
                ProductId = sellerAndProductResult.ProductId,
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

        public GetSellerIdResult GetSellerIdWithUserId(int UserId)
        {
            var seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted
            && x.UserId == UserId
            && x.Status == Data.Enums.Status.Approved);

            if (seller == null)
            {
                return new GetSellerIdResult()
                {
                    Success = false
                };
            }

            return new GetSellerIdResult()
            {
                Success = true,
                SellerId = seller.Id
            };
        }

        public GetSellerIdAndProductIdResult GetSellerIdWithUserIdAndProductId(int UserId,int ProductId)
        {
            var seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted
            && x.UserId == UserId
            && x.Status == Data.Enums.Status.Approved);

            var product = _context.Product.FirstOrDefault(x => !x.IsDeleted
            && x.SellerId == seller.Id
            && x.Id == ProductId);
            
            if (product == null)
            {
                return new GetSellerIdAndProductIdResult()
                {
                    Success = false
                };
            }

            return new GetSellerIdAndProductIdResult()
            {
                Success = true,
                SellerId = seller.Id,
                ProductId = product.Id
            };
        }
    }
}

