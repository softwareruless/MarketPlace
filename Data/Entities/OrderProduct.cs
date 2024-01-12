using System;
using System.ComponentModel.DataAnnotations.Schema;
using MarketPlace.Data.Enums;

namespace MarketPlace.Data.Entities
{
	public class OrderProduct : BaseItemEntity
	{
		[ForeignKey("OrderId")]
		public int OrderId { get; set; }

		[ForeignKey("ProductId")]
		public int ProductId { get; set; }

		public int SellerId { get; set; }

		public double UnitPrice { get; set; }
		public int Count { get; set; }
		public double TotalPrice { get; set; }

		public string ShippingUrl { get; set; }
		public string ShippingCode { get; set; }

		public DateTime DeliveredDate { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}

