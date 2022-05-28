using Authentication.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataAccess.DbContext
{
    public class AuthDbContext : IdentityDbContext<User>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

            //builder.HasDefaultSchema("Auth");

            builder.Entity<User>(e =>
            {
                e.HasOne(x => x.Account).WithOne(x => x.User).HasForeignKey<Account>(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Address>(e =>
            {
                e.HasOne(x => x.Account).WithMany(q => q.Addresses).HasForeignKey(q => q.AccountId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<User>().Navigation(e => e.Account).AutoInclude();
        }


    }
}
