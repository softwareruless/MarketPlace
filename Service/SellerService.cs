using AutoMapper;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Enums;
using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Utilities;

namespace MarketPlace.Service
{
    public class SellerService : ISellerService
    {
        private readonly MarketPlaceDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public SellerService(MarketPlaceDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateSeller(SellerAddModel model, int UserId)
        {
            var ppUploadResult = await UploadPhotoHelper.UploadSellerProfilePhoto(model.ProfilePhoto);

            if (!ppUploadResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.ProfileUpload"]
                };
            }

            var coverUploadResult = await UploadPhotoHelper.UploadSellerCover(model.CoverPhoto);

            if (!coverUploadResult.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.CoverUpload"]
                };
            }

            Seller seller = _mapper.Map<Seller>(model);
            seller.UserId = UserId;
            seller.Status = Status.Pending;
            seller.CoverPhoto = coverUploadResult.Path;
            seller.ProfilePhoto = ppUploadResult.Path;

            _context.Seller.Add(seller);
            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.CreateSeller"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };


        }

        public async Task<ResponseModel> UpdateSeller(SellerUpdateModel model, int UserId)
        {

            Seller seller = _context.Seller.FirstOrDefault(x => x.Id == model.Id && x.UserId == UserId);
            string oldProfile = String.Empty;
            string oldCover = String.Empty;
            if (seller == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.SellerNotFound"]
                };
            }

            if (model.ProfilePhoto != null)
            {
                oldProfile = seller.ProfilePhoto;
                var ppUploadResult = await UploadPhotoHelper.UploadSellerProfilePhoto(model.ProfilePhoto);

                if (!ppUploadResult.Success)
                {
                    return new ResponseModel()
                    {
                        Success = false,
                        Message = _configuration["SellerService.ProfileUpload"]
                    };
                }

                seller.ProfilePhoto = ppUploadResult.Path;
            }

            if (model.CoverPhoto != null)
            {
                oldCover = seller.CoverPhoto;

                var coverUploadResult = await UploadPhotoHelper.UploadSellerCover(model.CoverPhoto);

                if (!coverUploadResult.Success)
                {
                    return new ResponseModel()
                    {
                        Success = false,
                        Message = _configuration["SellerService.CoverUpload"]
                    };
                }

                seller.CoverPhoto = coverUploadResult.Path;
            }

            seller.Name = model.Name;
            seller.Description = model.Description;

            _context.Seller.Add(seller);
            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.UpdateSeller"]
                };
            }

            // todo check deleting process old cover and pp ==> check values oldCover and oldProfile

            if (!string.IsNullOrEmpty(oldCover))
            {
                UploadPhotoHelper.DeletePhoto(oldCover, "SellerCover");
            }

            if (!string.IsNullOrEmpty(oldProfile))
            {
                UploadPhotoHelper.DeletePhoto(oldProfile, "SellerProfilePhotos");
            }

            return new ResponseModel()
            {
                Success = true
            };


        }

        public ResponseModel DeleteSeller(int SellerId, int UserId)
        {
            Seller seller = _context.Seller.FirstOrDefault(x => !x.IsDeleted && x.Id == SellerId && x.UserId == UserId);

            seller.IsDeleted = true;
            seller.Status = Status.Pending;

            _context.Seller.Update(seller);

            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.DeleteSeller"]
                };
            }

            return new ResponseModel()
            {
                Success = true
            };

        }

        public SellerDetailResponseModel GetSellerDetail(int SellerId)
        {
            var seller = _context.Seller.Where(x => x.Id == SellerId && x.Status == Status.Approved).Select(x => new SellerDetail()
            {
                Id = x.Id,

                Name = x.Name,
                Description = x.Description,
                FirstName = x.FirstName,
                Surname = x.Surname,
                ProfilePhoto = x.ProfilePhoto,
                CoverPhoto = x.CoverPhoto,
            }).FirstOrDefault();

            if (seller == null)
            {
                return new SellerDetailResponseModel()
                {
                    Success = false,
                    Message = _configuration["SellerService.SellerNotFound"]
                };
            }

            return new SellerDetailResponseModel()
            {
                Success = true,
                Seller = seller
            };
        }

        public DatatableModel<SellerDetail> GetSellers(PagingParams pagingParams)
        {
            var sellers = _context.Seller.Where(x => !x.IsDeleted).AsQueryable();

            if (!String.IsNullOrEmpty(pagingParams.searchValue))
            {
                sellers = sellers.Where(x => x.Name.ToLower() == pagingParams.searchValue);
            }

            var filteredSellers = sellers.
            Select(x => new SellerDetail()
            {
                Id = x.Id,

                Name = x.Name,
                Description = x.Description,

                FirstName = x.FirstName,
                Surname = x.Surname,

                ProfilePhoto = x.ProfilePhoto,
                CoverPhoto = x.CoverPhoto,
            }).ToList();

            pagingParams.recordsTotal = (filteredSellers != null) ? filteredSellers.Count() : 0;
            var data = filteredSellers.Skip(pagingParams.skip).Take(pagingParams.pageSize).ToList();
            var jsonData = new DatatableModel<SellerDetail> { draw = pagingParams.draw, recordsFiltered = data.Count(), recordsTotal = pagingParams.recordsTotal, data = data };

            return jsonData;
        }

        //public async Task<ResponseModel> AddDocument(DocumentAddModel model,int UserId)
        //{
        //    var seller = _context.Seller.FirstOrDefault(x => x.Id == model.SellerId && x.UserId == UserId);

        //    if (seller == null)
        //    {
        //        return new ResponseModel()
        //        {
        //            Success = true,
        //            Message = _configuration["SellerService.SellerNotFound"]
        //        };
        //    }





        //}


    }
}

