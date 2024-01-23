using AutoMapper;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;
using System.Net;

namespace MarketPlace.Utilities
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserAddModel ,User>();
            CreateMap<UpdateNameModel, User>();
            CreateMap<ProductAddModel, Product>();
            CreateMap<SellerAddModel, Seller>();
        }
    }
}
