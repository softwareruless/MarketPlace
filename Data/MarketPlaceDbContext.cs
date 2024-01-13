using MarketPlace.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MarketPlace.Data
{
    public class MarketPlaceDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options)
        {

        }

        public DbSet<EmailVerification> EmailVerification { get; set; }

        public DbSet<Seller> Seller { get; set; }
        public DbSet<SellerDocument> SellerDocument { get; set; }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductPhoto> ProductPhoto { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<CommentPhoto> CommentPhoto { get; set; }
        public DbSet<Favourite> Favourite { get; set; }
        public DbSet<FeaturedProduct> FeaturedProduct { get; set; }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
        public DbSet<Payment> Payment { get; set; }

        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<Card> Card { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<District> District { get; set; }

        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        
    }
}
