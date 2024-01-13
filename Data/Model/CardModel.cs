namespace MarketPlace.Data.Model
{
	public class CardAddModel
	{
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
        public string FullName { get; set; }

        public string CardName { get; set; }
    }

    public class CardUpdateModel
    {
        public int Id { get; set; }

        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
        public string FullName { get; set; }

        public string CardName { get; set; }
    }
}

