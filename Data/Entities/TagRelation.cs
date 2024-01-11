using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class TagRelation : BaseEntity
    {
        [ForeignKey("BlogId")]
        public int BlogId { get; set; }

        [ForeignKey("TagId")]
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}

