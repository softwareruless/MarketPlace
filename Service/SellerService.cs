using System;
using AutoMapper;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Model;

namespace MarketPlace.Service
{
	public class SellerService : ISellerService
	{
        private readonly MarketPlaceDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public SellerService(MarketPlaceDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

    }
}

