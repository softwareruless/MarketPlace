using System;
using MarketPlace.Data.Enums;

namespace MarketPlace.Data.Model
{
	public class SellerAddModel
	{
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string TaxNumber { get; set; }
        public string TaxBuilding { get; set; }
        public string Description { get; set; }
        public string Iban { get; set; }

        public IFormFile ProfilePhoto { get; set; }
        public IFormFile CoverPhoto { get; set; }

        public Status Status { get; set; }
    }

    public class SellerUpdateModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string TaxNumber { get; set; }
        public string TaxBuilding { get; set; }
        public string Description { get; set; }
        public string Iban { get; set; }

        public IFormFile? ProfilePhoto { get; set; }
        public IFormFile? CoverPhoto { get; set; }

        public Status Status { get; set; }
    }

    public class SellerDetail
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string TaxNumber { get; set; }
        public string TaxBuilding { get; set; }
        public string Description { get; set; }
        public string Iban { get; set; }

        public IFormFile ProfilePhoto { get; set; }
        public IFormFile CoverPhoto { get; set; }

        public Status Status { get; set; }
    }

    public class SellerDetailWithUserId : SellerDetail
    {
        public int UserId { get; set; }
    }

    public class SellerDetailResponseModel : ResponseModel
    {
        public ProductDetail Product { get; set; }
    }

    public class SellersDetailResponseModel : ResponseModel
    {
        public List<ProductDetail> Products { get; set; }
    }

}

