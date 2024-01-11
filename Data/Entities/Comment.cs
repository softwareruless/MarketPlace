using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class Comment : BaseItemEntity
	{
		[ForeignKey("ProductId")]
		public int ProductId { get; set; }

		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public string Text { get; set; }
		public int Rate { get; set; }
	}
}

