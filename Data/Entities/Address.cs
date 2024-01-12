using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class Address : BaseItemEntity
    {
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public int CityId { get; set; }
        public int DistrictId { get; set; }

        public string Name { get; set; }
        public string AddressText { get; set; }
        public string Number { get; set; }
        public string FullName { get; set; }
        public string BuildingName { get; set; }
    }
}

