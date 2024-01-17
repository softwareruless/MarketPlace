using System;
namespace MarketPlace.Data.Model
{
	public class GetSellerIdResult : ResponseModel
	{
		public int SellerId { get; set; }
	}

    public class GetSellerIdAndProductIdResult : ResponseModel
    {
        public int SellerId { get; set; }
        public int ProductId { get; set; }
    }
}

