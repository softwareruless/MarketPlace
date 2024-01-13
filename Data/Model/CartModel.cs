
namespace MarketPlace.Data.Model
{
	public class CartAddModel
	{
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }

    public class CartUpdateModel
    {
        public int Id { get; set; }
        public bool Increase { get; set; }
        public int Count { get; set; }
    }
}

