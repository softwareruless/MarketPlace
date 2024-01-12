using System;
using System.ComponentModel.DataAnnotations.Schema;
using MarketPlace.Data.Enums;

namespace MarketPlace.Data.Entities
{
	public class Order : BaseItemEntity
	{
		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public int CityId { get; set; }
		public int DistrictId { get; set; }

		public string Address { get; set; }

		public double TotalPrice { get; set; }

		public DateTime DeliveredDate { get; set; }

		public int? CouponId { get; set; }
		public double CouponTotal { get; set; }

		public string CardNumber { get; set; }
		public string ExpiryDate { get; set; }
		public string SecurityNumber { get; set; }
		public string FullName { get; set; }

		public bool IsFinished { get; set; }
	}
}

