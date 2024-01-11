using MarketPlace.Data.Entities;

namespace MarketPlace.Data.Model.ReturnModel
{
    public class BlogDetail
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFeatured { get; set; }
        public string Date { get; set; }
        public List<TagRelation> Tags{ get; set; }
    }

    public class BlogDetailReturnModel : ResponseModel
    {
        public BlogDetail Blog { get; set; }
    }

    public class BlogsDetailReturnModel : ResponseModel
    {
        public List<BlogDetail> Blogs { get; set; }
    }
}
