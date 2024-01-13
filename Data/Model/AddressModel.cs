using System;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Data.Model
{
	public class AddressAddModel
	{
        public int CityId { get; set; }
        public int DistrictId { get; set; }

        public string Name { get; set; }
        public string AddressText { get; set; }
        public string Number { get; set; }
        public string FullName { get; set; }
        public string BuildingName { get; set; }
    }

    public class AddressUpdateModel
    {
        public int Id { get; set; }

        public int CityId { get; set; }
        public int DistrictId { get; set; }

        public string Name { get; set; }
        public string AddressText { get; set; }
        public string Number { get; set; }
        public string FullName { get; set; }
        public string BuildingName { get; set; }
    }
}

