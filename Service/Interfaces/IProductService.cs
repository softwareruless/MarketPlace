using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;

namespace MarketPlace.Service
{
	public interface IProductService
	{
        ResponseModel CreateProduct(ProductAddModel model, int UserId);
        ResponseModel UpdateProduct(ProductUpdateModel model, int UserId);
        ResponseModel DeleteProduct(int productId, int UserId);
        Task<ResponseModel> AddPhoto(PhotoAddModel model, int UserId);
        ResponseModel DeletePhoto(int PhotoId, int UserId);
        ProductDetailResponseModel GetProduct(int productId);
        DatatableModel<ProductDetail> GetProducts(PagingParams pagingParams);
    }
}

