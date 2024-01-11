using System;
namespace MarketPlace.Data.Model
{
    public class TagModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class TagDetailReturnModel : ResponseModel
    {
        public TagModel Tag { get; set; }
    }

    public class TagsDetailReturnModel : ResponseModel
    {
        public List<TagModel> Tags { get; set; }
    }

}

