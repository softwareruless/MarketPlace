using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class Favourite : BaseItemEntity
	{
		[ForeignKey("ProductId")]
		public int ProductId { get; set; }

		[ForeignKey("UserId")]
		public int UserId { get; set; }
	}
}

