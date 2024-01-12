using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class Cart : BaseItemEntity
	{
		[ForeignKey("UserId")]
		public int UserId { get; set; }

		[ForeignKey("ProductId")]
		public int ProductId { get; set; }

		public int Count { get; set; }
	}
}
