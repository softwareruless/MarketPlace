using System;
namespace MarketPlace.Data.Entities
{
	public class CommentPhoto : BaseItemEntity
	{
		public int CommentId { get; set; }
		public string PhotoUrl { get; set; }
	}
}

