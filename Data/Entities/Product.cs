using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class Product : BaseItemEntity
	{
        [ForeignKey("SellerId")]
        public int SellerId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public int BrandId { get; set; }
		public int CategoryId { get; set; }
	}
}

