using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class FeaturedProduct
	{
		[ForeignKey("ProductId")]
		public int ProductId { get; set; }

		public DateTime EndDate { get; set; }
	}
}

