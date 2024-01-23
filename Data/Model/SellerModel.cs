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
    }

    public class SellerUpdateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        //public string FirstName { get; set; }
        //public string Surname { get; set; }
        //public string TaxNumber { get; set; }
        //public string TaxBuilding { get; set; }
        public string Description { get; set; }
        //public string Iban { get; set; }

        public IFormFile? ProfilePhoto { get; set; }
        public IFormFile? CoverPhoto { get; set; }
    }

    public class SellerDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }

        public string ProfilePhoto { get; set; }
        public string CoverPhoto { get; set; }

        public Status Status { get; set; }
    }

    public class SellerDetailWithUserId : SellerDetail
    {
        public int UserId { get; set; }
    }

    public class SellerDetailResponseModel : ResponseModel
    {
        public SellerDetail Seller { get; set; }
    }

    public class SellersDetailResponseModel : ResponseModel
    {
        public List<SellerDetail> Sellers { get; set; }
    }

    public class DocumentAddModel
    {
        public IFormFile Document { get; set; }
        public DocType DocType { get; set; }
    }

    public class DocumentsAddModel
    {
        public int SellerId { get; set; }
        public List<DocumentAddModel> Documents { get; set; }
    }

    public class DocumentsResponseModel : ResponseModel
    {
        public List<DocumentResponse> Docs { get; set; }
    }

    public class DocumentResponse
    {
        public string Path { get; set; }
        public DocType DocType { get; set; }
    }


}

