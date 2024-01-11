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

        public DbSet<Blog> Blog { get; set; }
        public DbSet<EmailVerification> EmailVerification { get; set; }
        public DbSet<ContactForm> ContactForm { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TagRelation> TagRelation { get; set; }




    }
}
