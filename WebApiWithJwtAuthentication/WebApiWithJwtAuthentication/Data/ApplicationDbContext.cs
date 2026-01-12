using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiWithJwtAuthentication.Models.Identity;

namespace WebApiWithJwtAuthentication.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //or public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //1st checks RefreshToken class when we write has one, when WithMany it checks User class
            builder.Entity<RefreshToken>().HasOne(rt => rt.User).WithMany(u => u.RefreshTokens).HasForeignKey(rt => rt.UserId);
        }
    }
}
