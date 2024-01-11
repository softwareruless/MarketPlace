using System.ComponentModel.DataAnnotations.Schema;
using MarketPlace.Data.Enums;

namespace MarketPlace.Data.Entities
{
    public class SellerDocument : BaseItemEntity
	{
		public string Path { get; set; }

		//[ForeignKey("SellerId")]
		public int SellerId { get; set; }
		public DocType DocType { get; set; }
	}
}

