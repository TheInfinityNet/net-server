using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Identity.Infrastructure.Data
{
    public class IdentityDbContext : PostreSqlDbContext<IdentityDbContext>
    {

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountProvider> AccountProviders { get; set; }

        public DbSet<Verification> Verifications { get; set; }


        public IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountProvider>()
                .HasOne(p => p.Account)
                .WithMany(b => b.AccountProviders)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verification>()
                .HasOne(p => p.Account)
                .WithMany(b => b.Verifications)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
