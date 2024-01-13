namespace MarketPlace.Data.Entities
{
	public class Campaign : BaseItemEntity
	{
		public int SellerId { get; set; }
		public int? ProductId { get; set; }

		//public CampaignType CampaignType { get; set; }
		//public CampaignType CampaignType { get; set; }

		public int ProductLimit { get; set; }
		public int TotalLimit { get; set; }

		public int DiscountRate { get; set; }
		public double DiscountPrice { get; set; }

		public bool IsFinished { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime FinishDate { get; set; }
	}
}

