using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class Coupon : BaseItemEntity
	{
		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public int DiscountRate { get; set; }
		public double DiscountPrice { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

		public bool IsUsed { get; set; }
	}
}

