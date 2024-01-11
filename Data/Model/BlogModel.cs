using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Data.Model
{
    public class BlogAddModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFeatured { get; set; }
        public List<int> TagRelations { get; set; }
    }

    public class BlogUpdateModel
    {
        [Required]
        public int Id{ get; set; }
        [Required]
        public string Title { get; set; }

        public IFormFile Photo { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFeatured { get; set; }
        public List<int> TagRelations { get; set; }

    }


}
