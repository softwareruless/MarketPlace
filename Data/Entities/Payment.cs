using System;
using System.ComponentModel.DataAnnotations.Schema;
using MarketPlace.Data.Enums;

namespace MarketPlace.Data.Entities
{
	public class Payment : BaseItemEntity
	{
		[ForeignKey("UserId")]
		public int UserId { get; set; }

        public double TotalPrice { get; set; }
        public double VatRate { get; set; }

        public DateTime DeliveredDate { get; set; }

        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityNumber { get; set; }
        public string FullName { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}

