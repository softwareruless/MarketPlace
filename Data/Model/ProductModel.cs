using System;
namespace MarketPlace.Data.Model
{
    public class ProductAddModel
    {
        public int SellerId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductUpdateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }

    public class PhotoAddModel
    {
        public int ProductId { get; set; }

        public List<IFormFile> Photos { get; set; }
    }

    public class PhotosResponseModel : ResponseModel
    {
        public List<string> Names { get; set; }
    }
    
    public class ProductDetail
    {
        public int SellerId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductDetailResponseModel : ResponseModel
    {
        public ProductDetail Product { get; set; }
    }

    public class ProductsDetailResponseModel : ResponseModel
    {
        public List<ProductDetail> Products { get; set; }
    }
}

