using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class FeaturedProduct : BaseItemEntity
	{
		[ForeignKey("ProductId")]
		public int ProductId { get; set; }

		public DateTime EndDate { get; set; }
	}
}

