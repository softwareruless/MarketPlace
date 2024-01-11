using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class Tag : BaseItemEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}

