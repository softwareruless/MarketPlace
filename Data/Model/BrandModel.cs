using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Data.Model
{
	public class BrandAddModel
	{
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string Desription { get; set; }
    }

    public class BrandUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string Desription { get; set; }
    }
}

