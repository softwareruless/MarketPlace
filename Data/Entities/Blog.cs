namespace MarketPlace.Data.Entities
{
    public class Blog : BaseItemEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFeatured { get; set; }

        public virtual List<TagRelation> Tags { get; set; }
    }
}
