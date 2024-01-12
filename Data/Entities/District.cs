using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class District : BaseEntity
    {
        [ForeignKey("CityId")]
        public int CityId { get; set; }
        public string Name { get; set; }
    }
}

