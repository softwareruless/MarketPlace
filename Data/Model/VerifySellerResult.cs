using System;
using MarketPlace.Data.Entities;

namespace MarketPlace.Data.Model
{
	public class GetSellerIdResult : ResponseModel
	{
		public int SellerId { get; set; }
	}

    public class SellerVerifyResult : ResponseModel
    {
        public Seller Seller { get; set; }
    }

    public class ProductVerifyResult : SellerVerifyResult
    {
        public Product Product { get; set; }
    }

    public class ProductPhotoVerifyResult : ProductVerifyResult
    {
        public ProductPhoto PPhoto { get; set; }
    }
}

