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
            CreateMap<BlogAddModel, Blog>();
            CreateMap<BlogUpdateModel, Blog>();
            CreateMap<ContactMessageAddModel, ContactForm>();
            CreateMap<TagModel, Tag>();
        }
    }
}
