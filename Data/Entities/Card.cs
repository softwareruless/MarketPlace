using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class Card : BaseItemEntity
	{
		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public string CardNumber { get; set; }
		public string ExpiryDate { get; set; }
		public string SecurityCode { get; set; }
		public string FullName { get; set; }

		public string CardName { get; set; }
	}
}

